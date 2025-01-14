using System;
using Microsoft.AspNetCore.Http;

namespace FoodMart.BL.VM.Slider;

public class SliderUpdateVM
{
    public string Title { get; set; }
    public string UpTitle { get; set; }
    public string ImageUrl { get; set; }
    public string ExistingImageUrl { get; set; }
    public string Desciption { get; set; }
    public IFormFile File { get; set; }
}

