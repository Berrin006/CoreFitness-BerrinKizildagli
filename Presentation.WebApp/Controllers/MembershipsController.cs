using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;
using Infrastructure.Identity;

namespace Presentation.WebApp.Controllers;

public class MembershipsController(IMembershipService membershipService, UserManager<ApplicationUser> userManager) : Controller
{
    [MenuItem("Memberships", 5)]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);

        var currentMembership = user != null
            ? await membershipService.GetMembershipByUserIdAsync(user.Id)
            : null;

        return View(currentMembership);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Subscribe(string planName)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Register", "Account", new { returnUrl = Url.Action("Index", "Memberships") });
        }

        var success = await membershipService.CreateMembershipAsync(user.Id, planName);

        if (success)
        {
            TempData["SuccessMessage"] = $"Success! You are now a {planName} member.";
            return RedirectToAction("Account", "Account");
        }

        TempData["ErrorMessage"] = "Could not process your membership. Please try again.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize] 
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel()
    {
        var user = await userManager.GetUserAsync(User);
        if (user != null)
        {
            await membershipService.CancelMembershipAsync(user.Id);
        }

        return RedirectToAction("Account", "Account");
    }
}