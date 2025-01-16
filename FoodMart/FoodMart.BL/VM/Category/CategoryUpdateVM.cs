using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FoodMart.BL.VM.Category;
public class CategoryUpdateVM
{
    [Required(ErrorMessage = "Name is required!"), MaxLength(64, ErrorMessage = "Category name must be less than 64 charachters")]
    public string Name { get; set; }
    public string? ExistingImageUrl { get; set; }
    public IFormFile? Image { get; set; }
}

