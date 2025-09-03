using Microsoft.AspNetCore.Mvc;

namespace Admin_Panel.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
