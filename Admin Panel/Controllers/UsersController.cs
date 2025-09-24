using Admin_Panel.Models.UserVMs;
using Admin_Panel.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Policy;
using Talabat.Core.Domain_Models.Identity;
using Talabat.Repository.Identity;

namespace Admin_Panel.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationIdentityContext identityContext;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, ApplicationIdentityContext identityContext, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.identityContext = identityContext;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            List<UsersVM> users_vms = new List<UsersVM>();
            foreach (var user in userManager.Users.Include(U => U.Address))
                users_vms.Add(new UsersVM
                {
                    ID = user.Id,
                    Email = user.Email,
                    Name = user.DispalyName,
                    Roles = (await userManager.GetRolesAsync(user)).ToList(),
                    Address = user.Address is not null ? string.Concat(user.Address.Street, "-", user.Address.City, "-", user.Address.Country) : "---"
                });
            return View(users_vms);
        }

        public async Task<IActionResult> UserCreationAsync()
        {
            var privilegeCheckResult = await PrivilegesChecker.CheckAsync(HttpContext, Privilege.UsersCreation);
            if (!privilegeCheckResult.isAllowed)
            {
                TempData["Message"] = privilegeCheckResult.Message;
                TempData["MessageColor"] = "danger";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserCreation(UserCreationVM userCreationVM)
        {
            if (!ModelState.IsValid)
                return View();
            ApplicationUser user = await userManager.FindByEmailAsync(userCreationVM.Email);
            if(user is not null)
            {
                ModelState.AddModelError("Already existing", "There is a user with that email");
                return View();
            }
            user = new ApplicationUser
            {
                Email = userCreationVM.Email,
                DispalyName = string.Concat(userCreationVM.FirstName," ",userCreationVM.LastName),
                UserName = userCreationVM.Email.Split('@')[0],
                Address = new Address
                {
                    FirstName = userCreationVM.FirstName,
                    LastName = userCreationVM.LastName,
                    City = userCreationVM.City,
                    Street = userCreationVM.Street,
                    Country = userCreationVM.Country
                }
            };
            var result = await userManager.CreateAsync(user, userCreationVM.Password);
            TempData["Message"] = result.Succeeded ? "✅ User created successfully!" : "❌ A problem occurred while creating the user";
            TempData["MessageColor"] = result.Succeeded ? "success" : "danger";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string ID)
        {
            var privilegeCheckResult = await PrivilegesChecker.CheckAsync(HttpContext, Privilege.Users_EditDelete);
            if (!privilegeCheckResult.isAllowed)
            {
                TempData["Message"] = privilegeCheckResult.Message;
                TempData["MessageColor"] = "danger";
                return RedirectToAction("Index");
            }
            var user = await userManager.FindByIdAsync(ID);
            if(user is not null)
            {
                // If that targeted user isn't the admin
                if (user.Email != "admin000@talabat.com")
                {
                    var CurrentUserEmail = (await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier))).Email;
                    var result = await userManager.DeleteAsync(user);
                    TempData["Message"] = result.Succeeded ? "✅ User deleted successfully!" : "❌ A problem occurred while deleting the user";
                    TempData["MessageColor"] = result.Succeeded ? "success" : "danger";

                    
                    // That user deleted himself => Sign him out
                    if (CurrentUserEmail == user.Email)
                        return RedirectToAction("SignOut", "Account");
                }
                else
                {
                    TempData["Message"] = "❌ The admin account musn't be deleted";
                    TempData["MessageColor"] = "danger";
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditUser(string ID)
        {
            var privilegeCheckResult = await PrivilegesChecker.CheckAsync(HttpContext, Privilege.Users_EditDelete);
            if (!privilegeCheckResult.isAllowed)
            {
                TempData["Message"] = privilegeCheckResult.Message;
                TempData["MessageColor"] = "danger";
                return RedirectToAction("Index");
            }
            var user = userManager.Users.Where(U => U.Id == ID).Include(U => U.Address).AsNoTrackingWithIdentityResolution().FirstOrDefault();
            var roles = roleManager.Roles.Select(R => R.Name).ToList();
            var userEditVM = new UserEditVM
            {
                ID = user.Id,
                FirstName = user.Address?.FirstName,
                LastName = user.Address?.LastName,
                Email = user.Email,
                Street = user.Address?.Street,
                City = user.Address?.City,
                Country = user.Address?.Country
            };
            foreach (var role in roles)
                userEditVM.isInRole.Add(new Pair<string, bool>(role, await userManager.IsInRoleAsync(user, role)));
            return View(userEditVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserEditVM userEditVM)
        {
            if(!ModelState.IsValid)
                return View(userEditVM);

            var TargetUser = await userManager.FindByIdAsync(userEditVM.ID);

            // Checking if the user doesn't exist
            bool successFlag = TargetUser is not null;

            var userWithTheEmail = await userManager.FindByEmailAsync(userEditVM.Email);
            // The updated email is of another user
            if(successFlag && userWithTheEmail is not null && TargetUser.Id != userWithTheEmail.Id)
            {
                ModelState.AddModelError("Email is already in use", "There is an already exisitng user with that email");
                return View(userEditVM);
            }

            if(!string.IsNullOrEmpty(userEditVM.Password))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(TargetUser);
                successFlag = (await userManager.ResetPasswordAsync(TargetUser, token, userEditVM.Password) ).Succeeded;
            }
            
            if(successFlag)
            {
                TargetUser.DispalyName = string.Concat(userEditVM.FirstName, " ", userEditVM.LastName);
                TargetUser.Email = userEditVM.Email;
                identityContext.Entry(TargetUser).Reference(U => U.Address).Load();
                if (TargetUser.Address is null)
                {
                    TargetUser.Address = new Address();
                    TargetUser.Address.userID = TargetUser.Id;
                }
                TargetUser.Address.FirstName = userEditVM.FirstName;
                TargetUser.Address.LastName = userEditVM.LastName;
                TargetUser.Address.Street = userEditVM.Street;
                TargetUser.Address.City = userEditVM.City;
                TargetUser.Address.Country = userEditVM.Country;
            }

            successFlag &= (await userManager.UpdateAsync(TargetUser)).Succeeded;

            foreach(var pair in userEditVM.isInRole)
            {
                if (pair.Second)
                    await userManager.AddToRoleAsync(TargetUser, pair.First);
                else
                    await userManager.RemoveFromRoleAsync(TargetUser, pair.First);
            }    


            TempData["Message"] = successFlag ? "✅ User edited successfully!" : "❌ A problem occurred while editing the user";
            TempData["MessageColor"] = successFlag ? "success" : "danger";
            return RedirectToAction("Index");
        }
    }
}
