using System;
using FoodMart.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodMart.DAL.Context;
public class AppDbContext : IdentityDbContext<User>
{
	public DbSet<Slider> Sliders { get; set; }
	public DbSet<Product> Products{ get; set; }
	public DbSet<Category> Categories { get; set; }

	public AppDbContext(DbContextOptions opt) : base(opt) 
	{
	}

}

