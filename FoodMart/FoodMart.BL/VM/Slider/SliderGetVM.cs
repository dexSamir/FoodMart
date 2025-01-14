using System;
using Microsoft.AspNetCore.Http;

namespace FoodMart.BL.VM.Slider;
public class SliderGetVM
{
    public bool IsDeleted { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime? UpdatedTime { get; set; }
    public string Title { get; set; }
    public string UpTitle { get; set; }
    public string Desciption { get; set; }
    public IFormFile Image { get; set; }
}

