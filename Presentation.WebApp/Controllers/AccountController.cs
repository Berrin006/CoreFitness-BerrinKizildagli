using Application.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Account;
using System.Security.Claims;

namespace Presentation.WebApp.Controllers;

public class AccountController(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    IGymClassService gymClassService) : Controller
{
    [HttpGet]
    [Authorize]
    [Route("account")]
    public async Task<IActionResult> Account()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login");

        var userId = userManager.GetUserId(User);
        var myBookings = await gymClassService.GetBookedClassesForUserAsync(userId!);

        var viewModel = new AccountDetailsViewModel
        {
            FirstName = user.FirstName ?? user.UserName?.Split('@')[0] ?? "Firstname",
            LastName = user.LastName ?? "Lastname",
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber,
            BookedClasses = myBookings ?? new List<Domain.Aggregates.GymClasses.GymClass>()
        };

        return View(viewModel);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateDetails(AccountDetailsViewModel model)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        if (!ModelState.IsValid)
        {
            var userId = userManager.GetUserId(User);
            model.BookedClasses = await gymClassService.GetBookedClassesForUserAsync(userId!) ?? new List<Domain.Aggregates.GymClasses.GymClass>();
            return View("Account", model);
        }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;

        var result = await userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Profile has been updated!";
            return RedirectToAction("Account");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View("Account", model);
    }

    [HttpGet]
    public IActionResult Register(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.Email.Split('@')[0],
            LastName = "User",
            EmailConfirmed = true 
        };

        var result = await userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, isPersistent: true);
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your email and password.");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAccount()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        await signInManager.SignOutAsync();
        var result = await userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Your account has been deleted.";
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("Account");
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("account/set-password")]
    public async Task<IActionResult> SetPassword()
    {
        var user = await userManager.GetUserAsync(User);
        var viewModel = new SetPasswordViewModel
        {
            Email = user?.Email ?? ""
        };
        return View(viewModel);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("account/set-password")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "No user found with this email address.");
            return View(model);
        }

        var hasPassword = await userManager.HasPasswordAsync(user);
        if (hasPassword)
        {
            await userManager.RemovePasswordAsync(user);
        }

        var result = await userManager.AddPasswordAsync(user, model.Password);

        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Password has been set successfully! You can now log in.";
            return RedirectToAction("Login");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string? returnUrl = null)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        returnUrl ??= Url.Content("~/");
        if (remoteError != null)
        {
            ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
            return View("Login");
        }

        var info = await signInManager.GetExternalLoginInfoAsync();
        if (info == null) return RedirectToAction("Login");

        var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        if (result.Succeeded) return LocalRedirect(returnUrl);

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (email != null)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "User",
                    LastName = "External",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user);
            }
            await userManager.AddLoginAsync(user, info);
            await signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction("Login");
    }
}