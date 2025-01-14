using System;
using Microsoft.AspNetCore.Http;

namespace FoodMart.BL.Extension;
public static class FileExtension
{
	public static bool IsValidType(this IFormFile File, string type)
		=> File.ContentType.StartsWith(type);

	public static bool IsValidSize(this IFormFile File, int kb)
		=> File.Length <= kb * 1024;

	public static async Task<string> UploadAsync(this IFormFile file, params string[] paths)
	{
		string path = Path.Combine(paths);
		if (!Path.Exists(path))
			Directory.CreateDirectory(path);

		string fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
		using (Stream sr = File.Create(Path.Combine(path, fileName)))
			await file.CopyToAsync(sr);

		return fileName; 
	}
}

