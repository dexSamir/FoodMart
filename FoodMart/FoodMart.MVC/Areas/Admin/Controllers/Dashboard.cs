using Microsoft.AspNetCore.Mvc;

namespace FoodMart.MVC.Areas.Admin.Controllers;
[Area("Admin")]
public class Dashboard : Controller
{
    // GET: Dashboard
    public IActionResult Index()
    {
        return View();
    }

}
