﻿using System;
using FoodMart.Core.Entities.Base;

namespace FoodMart.Core.Entities;
public class Category : BaseEntity
{
	public string Name { get; set; }
	public string? ImageUrl { get; set; }
	public ICollection<Product> Products { get; set; } 
}

