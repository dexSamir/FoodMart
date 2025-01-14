using System;
using Microsoft.AspNetCore.Http;

namespace FoodMart.BL.VM.Slider;
public class SliderCreateVM
{
	public string Title { get; set; }
	public string UpTitle { get; set; }
	public string Desciption { get; set; }
	public IFormFile Image { get; set; }
}

