using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using FoodMart.BL.Extension;
using FoodMart.BL.VM.Product;
using FoodMart.Core.Entities;
using FoodMart.DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodMart.MVC.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController : Controller
{
    readonly AppDbContext _context;
    readonly IWebHostEnvironment _env;
    public ProductController(AppDbContext context, IWebHostEnvironment env)
    {
        _env = env; 
        _context = context; 
    }
    private async Task PopulateCategoriesAsync()
    {
        ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
    }

    private async Task<IActionResult> HandleModelErrorAsync(ProductCreateVM vm, string errorMessage, string key = "")
    {
        if (!string.IsNullOrEmpty(key))
            ModelState.AddModelError(key, errorMessage);
        else
            ModelState.AddModelError("", errorMessage);

        await PopulateCategoriesAsync();
        return View(vm);
    }

    private async Task<IActionResult> HandleModelErrorAsync(ProductUpdateVM vm, string errorMessage, string key = "")
    {
        if (!string.IsNullOrEmpty(key))
            ModelState.AddModelError(key, errorMessage);
        else
            ModelState.AddModelError("", errorMessage);

        await PopulateCategoriesAsync();
        return View(vm);
    }

    // GET: ProductController
    public async Task<IActionResult> Index()
    {
        return View(await _context.Products.Include(x=> x.Category).ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        await PopulateCategoriesAsync();
        return View(); 
    }
    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateVM vm)
    {
        if (!ModelState.IsValid)
            return await HandleModelErrorAsync(vm, "Model is not valid");

        if (await _context.Products.AnyAsync(x => x.Name == vm.Name))
            return await HandleModelErrorAsync(vm, "This name is already used!");

        if (vm.Image == null || vm.Image.Length == 0)
            return await HandleModelErrorAsync(vm, "File is required", "Image");

        if (!vm.Image.IsValidType("image"))
            return await HandleModelErrorAsync(vm, "File type must be an image", "Image");

        if (!vm.Image.IsValidSize(5))
            return await HandleModelErrorAsync(vm, "File size must be less than 5MB", "Image");


        Product product = new Product
        {
            CreatedTime = DateTime.UtcNow, 
            CostPrice = vm.CostPrice,
            CategoryId = vm.CategoryId,
            AvgRating = vm.AvgRating,
            SellPrice = vm.SellPrice,
            Quantity = vm.Quantity,
            Description = vm.Description,
            Name = vm.Name,
            ImageUrl = await vm.Image.UploadAsync(_env.WebRootPath, "imgs", "products")

        }; 
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index)); 
    }

    public async Task<IActionResult> Update(int? id)
    {
        if (!id.HasValue) return BadRequest();

        await PopulateCategoriesAsync();

        var data = await _context.Products
            .Where(x => x.Id == id)
            .Select(x => new ProductUpdateVM
            {
                Name = x.Name,
                Description = x.Description,
                CostPrice = x.CostPrice,
                SellPrice = x.SellPrice,
                Quantity = x.Quantity,
                CategoryId = x.CategoryId,
                ExistingImageUrl = x.ImageUrl, 
            }).FirstOrDefaultAsync();

        return View(data); 
    }
    [HttpPost]
    public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
    {
        if (!id.HasValue) return BadRequest();
        var data = await _context.Products.FindAsync(id);
        if (data == null) return NotFound();

        if(vm.Image != null)
        {
            if (await _context.Products.AnyAsync(x => x.Name == vm.Name))
                return await HandleModelErrorAsync(vm, "This name is already used!");

            if (!vm.Image.IsValidType("image"))
                return await HandleModelErrorAsync(vm, "File type must be an image", "Image");

            if (!vm.Image.IsValidSize(5))
                return await HandleModelErrorAsync(vm, "File size must be less than 5MB", "Image");

            string fileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs", "products", data.ImageUrl);
            if (System.IO.File.Exists(fileName))
            {
                try
                {
                    System.IO.File.Delete(fileName); 
                }
                catch(Exception ex)
                {
                    return StatusCode(500, $"Error deleting file {ex.Message}"); 
                }
            }
            string newFileName = await vm.Image.UploadAsync(_env.WebRootPath, "imgs", "products");
            data.ImageUrl = newFileName; 
        }

        data.CategoryId = vm.CategoryId;
        data.CostPrice = vm.CostPrice;
        data.SellPrice = vm.SellPrice;
        data.Name = vm.Name;
        data.AvgRating = vm.AvgRating;
        data.UpdatedTime = DateTime.UtcNow; 
        data.Description = vm.Description;
        data.Quantity = vm.Quantity;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index)); 
    }

    public async Task<IActionResult> ToggleProductVisibility(int? id, bool isdeleted)
    {
        if (!id.HasValue) return BadRequest();
        var data = await _context.Products.FindAsync(id);
        if (data == null) return NotFound();

        data.IsDeleted = isdeleted;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Hide(int? id)
       => await ToggleProductVisibility(id, true);

    public async Task<IActionResult> Show(int? id)
       => await ToggleProductVisibility(id, false);

    public async Task<IActionResult> Delete(int? id)
    {
        if (!id.HasValue) return BadRequest();
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        if (!string.IsNullOrEmpty(product.ImageUrl))
        {
            var filePath = Path.Combine(_env.WebRootPath, "imgs", "products", product.ImageUrl);
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error deleting file: {ex.Message}");
                }
            }
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
