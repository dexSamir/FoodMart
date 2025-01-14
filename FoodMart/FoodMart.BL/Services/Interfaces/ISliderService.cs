using System;
using FoodMart.BL.VM.Slider;

namespace FoodMart.BL.Services.Interfaces;
public interface ISliderService 
{
	Task CreateAsync(SliderCreateVM vm);
	Task UpdateAsync(SliderUpdateVM vm);
	Task RemoveAsync(int? id);
	Task ShowAsync(int? id);
    Task HideAsync(int? id);
}

