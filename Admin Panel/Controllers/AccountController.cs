using Admin_Panel.Models.AccountsVMs;
using Admin_Panel.Models.UserVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Talabat.Core.Domain_Models.Identity;
using Talabat.Repository.Identity;

namespace Admin_Panel.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginVM loginvm)
        {
            if (!ModelState.IsValid)
                return View("Index", loginvm);

            var user = await userManager.FindByEmailAsync(loginvm.Email);
            bool LogTheUserIn = user != null;

            LogTheUserIn = LogTheUserIn ? 
                            (await signInManager.PasswordSignInAsync(user, loginvm.Password, loginvm.RememberTheLogin, false)).Succeeded: 
                            false;
            
            // Either there is no user with that email || the password is wrong
            if (!LogTheUserIn)
            {
                ModelState.AddModelError("Invalid Login", "Wrong credentials");
                return View("Index", loginvm);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserCreationVM userCreationVM)
        {
            if (!ModelState.IsValid)
                return View(userCreationVM);
            ApplicationUser user = await userManager.FindByEmailAsync(userCreationVM.Email);
            if (user is not null)
            {
                ModelState.AddModelError("Already existing", "There is a user with that email");
                return View(userCreationVM);
            }
            user = new ApplicationUser
            {
                Email = userCreationVM.Email,
                DispalyName = string.Concat(userCreationVM.FirstName, " ", userCreationVM.LastName),
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

        public async Task<IActionResult> SignOutAsync()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
