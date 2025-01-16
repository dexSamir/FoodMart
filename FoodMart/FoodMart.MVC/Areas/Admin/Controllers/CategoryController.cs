using FoodMart.BL.Extension;
using FoodMart.BL.VM.Category;
using FoodMart.Core.Entities;
using FoodMart.DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodMart.MVC.Areas.Admin.Controllers;
[Area("Admin")]
public class CategoryController : Controller
{
    readonly AppDbContext _context;
    readonly IWebHostEnvironment _env; 
    public CategoryController(AppDbContext context, IWebHostEnvironment env)
    {
        _env = env; 
        _context = context; 
    }

    // GET: CategoryController
    public async Task<IActionResult> Index()
    {
        return View(await _context.Categories.ToListAsync());
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateVM vm) 
    {
        if (!ModelState.IsValid) return View();
        var data = await _context.Categories.FirstOrDefaultAsync(x => x.Name == vm.Name);
        if (data != null)
        {
            ModelState.AddModelError("", "A category with this name already exists.");
            return View(); 
        }
        if(vm.Image == null || vm.Image.Length == 0)
        {
            ModelState.AddModelError("Image", "Image Is required!");
            return View(); 
        }
        if(!vm.Image.IsValidType("image"))
        {
            ModelState.AddModelError("Image", "File type must be an image");
            return View();
        }
        if (!vm.Image.IsValidSize(5 * 1024))
        {
            ModelState.AddModelError("Image", "File size must be less than 5MB");
            return View();
        }
        Category category = new Category
        {
            Name = vm.Name,
            ImageUrl = await vm.Image.UploadAsync(_env.WebRootPath, "imgs", "categories") 
        };
        await _context.AddAsync(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int? id)
    {
        if (!id.HasValue) return BadRequest();
        var data = await _context.Categories
            .Where(x => x.Id == id)
            .Select(x => new CategoryUpdateVM
            {
                Name = x.Name,
                ExistingImageUrl = x.ImageUrl
            }).FirstOrDefaultAsync();

        if (data == null) return NotFound();
        return View(data); 
    }
    [HttpPost]
    public async Task<IActionResult> Update(int? id, CategoryUpdateVM vm)
    {
        if (!id.HasValue) return BadRequest();
        var data = await _context.Categories.FindAsync(id);
        if (data == null) return NotFound();

        if(vm.Image != null)
        {
            if (!vm.Image.IsValidType("image"))
            {
                ModelState.AddModelError("Image", "File type must be an image!");
                return View(vm);
            }
            if (!vm.Image.IsValidSize(5 * 1024))
            {
                ModelState.AddModelError("Image", "File size must be less than 5MB");
                return View(vm);
            }
            data.ImageUrl =await vm.Image.UploadAsync(_env.WebRootPath, "imgs", "categories"); 
        } 
        data.Name = vm.Name;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index)); 
    } 

    public async Task<IActionResult> ToggleCategoryVisibility(int? id, bool isdeleted)
    {
        if (!id.HasValue) return BadRequest();
        var data = await _context.Categories.FindAsync(id);
        if (data == null) return NotFound();

        data.IsDeleted = isdeleted;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Hide(int? id)
       => await ToggleCategoryVisibility(id, true);

    public async Task<IActionResult> Show(int? id)
       => await ToggleCategoryVisibility(id, false);

    public async Task<IActionResult> Delete(int? id)
    {
        if (!id.HasValue) return BadRequest();
        var data = await _context.Categories.FindAsync(id);
        if (data == null) return NotFound();

        var filename = Path.Combine(Directory.GetCurrentDirectory(), _env.WebRootPath, "imgs", "categories", data.ImageUrl);
        if (System.IO.Directory.Exists(filename))
            System.IO.File.Delete(filename); 

        _context.Remove(data);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));

    }
}
