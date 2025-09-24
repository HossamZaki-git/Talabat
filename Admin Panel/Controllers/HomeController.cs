using Admin_Panel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Domain_Models.Identity;
using Talabat.Repository.Data;

namespace Admin_Panel.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly StoreContext storeContext;

        public HomeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, StoreContext storeContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.storeContext = storeContext;
        }
        public IActionResult Index()
        {
            return View(new HomeVM
            {
                NumberOfUsers = userManager.Users.Count(),
                NumberOfRoles = roleManager.Roles.Count(),
                NumberOfProducts = storeContext.Products.Count()
            });
        }
    }
}
