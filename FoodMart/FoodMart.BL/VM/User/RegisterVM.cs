using System;
using System.ComponentModel.DataAnnotations;

namespace FoodMart.BL.VM.User;
public class RegisterVM
{
    [Required(ErrorMessage = "Name is required"), MaxLength(64, ErrorMessage = "Name length must be less than 64 charachters")]
	public string Name { get; set; }

    [Required(ErrorMessage = "Surname is required"), MaxLength(64, ErrorMessage = "Surname length must be less than 64 charachters")]
    public string Surname { get; set; }
	[Required, MaxLength(64)]
	public string Email { get; set; }

    [Required, MaxLength(64)]
    public string Username { get; set; }

	[Required, MaxLength(32), DataType(DataType.Password)]
	public string Password { get; set; }

    [Required, MaxLength(32), DataType(DataType.Password), Compare(nameof(Password))]
    public string RePassword { get; set; }
}

