using Microsoft.AspNetCore.Mvc;

namespace FoodMart.MVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public IActionResult Index()
        {
            return View();
        }
    }
}
