using System;
using FoodMart.Core.Repositories;
using FoodMart.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FoodMart.DAL;
public static class ServiceRegistration 
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISliderRepository, SliderRepository>();
        return services; 
    }
}

