using FoodMart.DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodMart.MVC.Controllers
{
    public class HomeController : Controller
    {
        readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context; 
        }

        // GET: HomeController
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.Where(x => !x.IsDeleted).ToListAsync()); 
        }
    }
}
