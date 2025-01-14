using System;
using FoodMart.Core.Entities;
using FoodMart.Core.Repositories;
using FoodMart.DAL.Context;
using Microsoft.AspNetCore.Http;

namespace FoodMart.DAL.Repositories;
public class SliderRepository : GenericRepository<Slider>, ISliderRepository
{
    public SliderRepository(AppDbContext context, IHttpContextAccessor http) : base(context, http)
    {
    }
}

