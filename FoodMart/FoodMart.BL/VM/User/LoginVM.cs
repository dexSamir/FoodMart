﻿using System;
namespace FoodMart.BL.VM.User;
public class LoginVM
{
	public string UsernameOrEmail { get; set; }
	public string Password { get; set; }
	public bool RememberMe { get; set; }
}

