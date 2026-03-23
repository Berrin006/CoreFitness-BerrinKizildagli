using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;

namespace Presentation.WebApp.Controllers;

[Authorize]
public class MembershipsController(IMembershipService membershipService, UserManager<IdentityUser> userManager) : Controller
{
    [MenuItem("Memberships", 5)] 
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        var currentMembership = await membershipService.GetUserMembershipAsync(user!.Id);

        return View(currentMembership);
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe(string planName)
    {
        var user = await userManager.GetUserAsync(User);
        var result = await membershipService.CreateMembershipAsync(user!.Id, planName);

        if (result.Success)
        {
            TempData["SuccessMessage"] = $"Grattis! Du är nu medlem med planen: {planName}";
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = "Kunde inte skapa medlemskap.";
        return RedirectToAction("Index");
    }
}