using System;
using FoodMart.Core.Entities;
using FoodMart.Core.Enums;
using FoodMart.DAL.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FoodMart.BL.Extension; 
public static class SeedExtension
{
	public static async void UseSeedExtension(this IApplicationBuilder app)
	{
		using (var scope = app.ApplicationServices.CreateScope())
		{
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			if(!roleManager.Roles.Any())
				foreach (var role in Enum.GetValues(typeof(Roles)))
					await roleManager.CreateAsync(new IdentityRole(role.ToString()));

			if(!userManager.Users.Any(x=> x.NormalizedUserName == "ADMIN"))
			{
				User user = new User
				{
					Name = "Samir",
					Surname = "Hebibov",
					UserName = "admin",
					Email = "admin@gmail.com",
				};

				await userManager.CreateAsync(user, "admin123");
				await userManager.AddToRoleAsync(user, nameof(Roles.Admin)); 
			}
		}
	}
}

