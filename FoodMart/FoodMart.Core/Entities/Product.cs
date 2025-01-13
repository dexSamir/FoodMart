using System;
using FoodMart.Core.Entities.Base;

namespace FoodMart.Core.Entities;
public class Product : BaseEntity 
{
	public string Name { get; set; }
	public string Description { get; set; }
	public decimal CostPrice { get; set; }
	public decimal SellPrice { get; set; }
	public int Quantity { get; set; }
	public string ImageUrl { get; set; }
	public double AvgRating { get; set; }
	public int CategoryId { get; set; }
	public Category Category { get; set; }
}

