using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FoodMart.BL.VM.Category;
public class CategoryCreateVM
{
	[Required(ErrorMessage = "Name is required!"), MaxLength(64, ErrorMessage = "Category name must be less than 64 charachters") ]
	public string Name { get; set; }
    public IFormFile? Image { get; set; }
}

