using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FoodMart.BL.VM.Product;
public class ProductCreateVM
{
    [Required(ErrorMessage = "Name is required!"), MaxLength(64, ErrorMessage = "Category name must be less than 64 charachters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Name is required!"), MaxLength(256, ErrorMessage = "Category name must be less than 256 charachters")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Cost Price is required!")]
    public decimal CostPrice { get; set; }

    [Required(ErrorMessage = "Sell Price is required!")]
    public decimal SellPrice { get; set; }

    [Required(ErrorMessage = "Quantity is required!")]
    public int Quantity { get; set; }

    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = "Average Rating is required!"), Range(1.0,5.0)]
    public double AvgRating { get; set; }

    [Required(ErrorMessage = "Category is required!")]
    public int CategoryId { get; set; }
}

