using FoodMart.BL.VM.User;
using FoodMart.Core.Entities;
using FoodMart.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodMart.MVC.Controllers;
public class AccountController : Controller
{
    public UserManager<User> _userManager;
    public SignInManager<User> _signInManager;
    bool isAuthenticated => User.Identity?.IsAuthenticated ?? false;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _signInManager = signInManager; 
        _userManager = userManager;
    }
    public IActionResult Register()
    {
        if (isAuthenticated) return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (isAuthenticated) return RedirectToAction("Index", "Home");

        if (!ModelState.IsValid) return View();

        if (vm.Password != vm.RePassword)
        {
            ModelState.AddModelError("", "Passwords do not match.");
            return View();
        }

        User user = new User
        {
            Name = vm.Name,
            Surname = vm.Surname,
            Email = vm.Email,
            UserName = vm.Username
        };

        var result = await _userManager.CreateAsync(user, vm.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(); 
        }

        var roleResult = await _userManager.AddToRoleAsync(user, nameof(Roles.Admin));
        if (!roleResult.Succeeded)
        {
            foreach (var error in roleResult.Errors)
                ModelState.AddModelError("", error.Description);
            return View();
        }
        return RedirectToAction(nameof(Login));
    }
    public ActionResult Login()
    {
        return View(); 
    }
    [HttpPost]
    public async Task<IActionResult> Login (LoginVM vm, string returnUrl = null)
    {
        if (isAuthenticated) return RedirectToAction("Index", "Home");

        if (!ModelState.IsValid) return View();

        User user = null;
        if (vm.UsernameOrEmail.Contains('@'))
            user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
        else
            user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);

        if(user == null)
        {
            ModelState.AddModelError("", "Username or password is wrong!");
            return View(); 
        }

        var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);

        if(!result.Succeeded)
        {
            if(result.IsNotAllowed)
                ModelState.AddModelError("", "Usernae or Password is wrong");
            if (result.IsLockedOut)
                ModelState.AddModelError("", "Wait until" + user.LockoutEnd!.Value.ToString("yyyy-MM-dd HH:mm:ss"));

            return View(); 
        }

        if(string.IsNullOrWhiteSpace(returnUrl))
        {
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Index", new { Controller = "Dashboard", Area = "Admin" });

            return RedirectToAction("Index", "Home"); 
        }
        return LocalRedirect(returnUrl);
    }
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login)); 
    }
}