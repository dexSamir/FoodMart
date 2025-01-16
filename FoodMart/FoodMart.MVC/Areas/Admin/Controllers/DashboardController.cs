using Microsoft.AspNetCore.Mvc;

namespace FoodMart.MVC.Areas.Admin.Controllers;
[Area("Admin")]
public class DashboardController : Controller
{
    // GET: Dashboard
    public IActionResult Index()
    {
        return View();
    }

}
