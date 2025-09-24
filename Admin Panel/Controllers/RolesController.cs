using Admin_Panel.Models;
using Admin_Panel.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models.Identity;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Admin_Panel.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly StoreContext storeContext;
        private readonly ApplicationIdentityContext identityContext;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, StoreContext storeContext, ApplicationIdentityContext identityContext)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.storeContext = storeContext;
            this.identityContext = identityContext;
        }


        public async Task<IActionResult> Index()
        {
            var roles = roleManager.Roles;
            Dictionary<string, int> roles_noOfMembers = new Dictionary<string, int>();
            foreach (var role in roles)
                roles_noOfMembers.Add(role.Name, 0);
            var users = userManager.Users;

            foreach (var user in users)
                foreach (var role in (await userManager.GetRolesAsync(user)))
                    roles_noOfMembers[role]++;

            return View(new RolesVM
            {
                Roles_NumberOfMembers = roles_noOfMembers
            });
        }

        public async Task<IActionResult> RoleCreation()
        {
            var privilegeCheckResult = await PrivilegesChecker.CheckAsync(HttpContext, Privilege.RolesCreation);
            if(!privilegeCheckResult.isAllowed)
            {
                TempData["Message"] = privilegeCheckResult.Message;
                TempData["MessageColor"] = "danger";
                return RedirectToAction("Index");
            }
            var vm = new RoleCreation_VM();
            foreach (Privilege privilege in Enum.GetValues(typeof(Privilege)))
                vm.Privileges.Add(new Utilities.Pair<Privilege, bool>(privilege, false));

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreation(RoleCreation_VM roleCreation_VM)
        {
            if(!ModelState.IsValid)
                return View();

            var role = await roleManager.FindByNameAsync(roleCreation_VM.RoleName);
            if(role is not null)
            {
                ModelState.AddModelError("Already existing", "That role name is already in use");
                return View();
            }

            var result = await roleManager.CreateAsync(new IdentityRole { Name= roleCreation_VM.RoleName.Trim()});

            string roleId = (await roleManager.FindByNameAsync(roleCreation_VM.RoleName)).Id;

            foreach (var pair in roleCreation_VM.Privileges)
                if (pair.Second)
                    identityContext.RolesPrivileges.Add(new RolePrivilege
                    {
                        privilege = pair.First,
                        RoleID = roleId
                    });
                    

            identityContext.SaveChanges();
            // Sending a message to the view
            TempData["Message"] = result.Succeeded ? "✅ Role created successfully!" : "❌ A problem occurred while creating the role";
            TempData["MessageColor"] = result.Succeeded ? "success" : "danger";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteRole([FromQuery] string RoleName)
        {
            var privilegeCheckResult = await PrivilegesChecker.CheckAsync(HttpContext, Privilege.Roles_EditDelete);
            if (!privilegeCheckResult.isAllowed)
            {
                TempData["Message"] = privilegeCheckResult.Message;
                TempData["MessageColor"] = "danger";
                return RedirectToAction("Index");
            }

            var role = await roleManager.FindByNameAsync(RoleName);
            var result = await roleManager.DeleteAsync(role);
            TempData["Message"] = result.Succeeded ? "✅ Role deleted successfully!" : "❌ A problem occurred while deleting the role";
            TempData["MessageColor"] = result.Succeeded ? "success" : "danger";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditRole(string roleName)
        {
            var privilegeCheckResult = await PrivilegesChecker.CheckAsync(HttpContext, Privilege.Roles_EditDelete);
            if (!privilegeCheckResult.isAllowed)
            {
                TempData["Message"] = privilegeCheckResult.Message;
                TempData["MessageColor"] = "danger";
                return RedirectToAction("Index");
            }

            var vm = new RoleEdit_VM
            {
                OldName = roleName,
                NewName = roleName
            };

            string roleId = (await roleManager.FindByNameAsync(roleName)).Id;
            var rolePrivileges = identityContext.RolesPrivileges.Where(RP => RP.RoleID == roleId).ToList();

            foreach (Privilege privilege in Enum.GetValues(typeof(Privilege)))
                vm.Privileges.Add(
                                    new Pair<Privilege, bool>(
                                                                privilege, 
                                                                rolePrivileges is not null && 
                                                                rolePrivileges.Any(RP => RP.privilege == privilege)
                                                                )
                                 );
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(RoleEdit_VM role_vm)
        {
            if (!ModelState.IsValid)
                return View();
            // There is an already existing role with the new name
            if(role_vm.NewName.Trim() != role_vm.OldName && await roleManager.FindByNameAsync(role_vm.NewName.Trim()) is not null)
            {
                ModelState.AddModelError("Already existing", "That name is already in use");
                return View(role_vm);
            }

            var OldRole = await roleManager.FindByNameAsync(role_vm.OldName);
            OldRole.Name = role_vm.NewName.Trim();

            bool flag = (await roleManager.UpdateAsync(OldRole)).Succeeded;

            bool privilegesChangeFlag = false;
            RolePrivilege rolePrivilege;
            Pair<Privilege, bool> privilegeFormPair = null;
            foreach (Privilege privilege in Enum.GetValues(typeof(Privilege)))
            {
                foreach(var p in role_vm.Privileges)
                    if(p.First == privilege)
                    {
                        // The <Privilege, bool> pair record of the form
                        privilegeFormPair = p;
                        break;
                    }
                // Fetching the RolePrivilege record from the db
                rolePrivilege = identityContext.RolesPrivileges.Where(RP => RP.privilege == privilege && RP.RoleID == OldRole.Id).FirstOrDefault();
                // Holds true if any change happens on any privilege of that role
                privilegesChangeFlag |= privilegeFormPair.Second ^ rolePrivilege is not null;
                // The user check that privilege && this isn't marked in the db
                if (privilegeFormPair.Second && rolePrivilege is null)
                    identityContext.Add(new RolePrivilege
                    {
                        privilege = privilege,
                        RoleID = OldRole.Id,
                    });
                // The user didn't check that privilege && is exists in the db
                else if(!privilegeFormPair.Second && rolePrivilege is not null)
                    identityContext.Remove(rolePrivilege);
            }

            // If a change happened on that role's privileges and the database didn't update it
            flag &= identityContext.SaveChanges() == 0 ^ privilegesChangeFlag;
            //var result = await roleManager.UpdateAsync(OldRole);
            TempData["Message"] = flag ? "✅ Role edited successfully!" : "❌ A problem occurred while editing the role";
            TempData["MessageColor"] = flag ? "success" : "danger";

            return RedirectToAction("Index");
        }
    }
}
