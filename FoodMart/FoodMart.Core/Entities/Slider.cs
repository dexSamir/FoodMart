using System;
using FoodMart.Core.Entities.Base;

namespace FoodMart.Core.Entities;
public class Slider : BaseEntity
{
	public string Title { get; set; }
	public string Description { get; set; }
	public string ImageUrl { get; set; }
	public string UpTitle { get; set; }
}

