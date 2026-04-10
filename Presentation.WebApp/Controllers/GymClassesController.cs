using Application.Services;
using Domain.Aggregates.GymClasses;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers;

public class GymClassesController(IGymClassService gymClassService, UserManager<ApplicationUser> userManager) : Controller
{
    [HttpGet]
    [Route("classes")]
    public async Task<IActionResult> Classes()
    {
        var classesResult = await gymClassService.GetAllAsync();
        var classes = classesResult.Value ?? new List<GymClass>();

        if (User.Identity!.IsAuthenticated)
        {
            var userId = userManager.GetUserId(User);
            var bookedIds = await gymClassService.GetBookedClassIdsForUserAsync(userId!);
            ViewData["BookedClassIds"] = bookedIds ?? new List<string>();
        }
        else
        {
            ViewData["BookedClassIds"] = new List<string>();
        }

        return View(classes);
    }

    [Authorize]
    [HttpGet]
    [Route("account/bookings")]
    public async Task<IActionResult> MyBookings()
    {
        var userId = userManager.GetUserId(User);
        var bookedClasses = await gymClassService.GetBookedClassesForUserAsync(userId!);
        var bookedIds = await gymClassService.GetBookedClassIdsForUserAsync(userId!);

        ViewData["BookedClassIds"] = bookedIds ?? new List<string>();
        ViewData["Title"] = "My Bookings";
        ViewData["IsMyBookingsPage"] = true;

        return View("Classes", bookedClasses ?? new List<GymClass>());
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(string id)
    {
        if (string.IsNullOrEmpty(id)) return BadRequest();
        var userId = userManager.GetUserId(User);
        var result = await gymClassService.BookClassAsync(userId!, id);

        if (!result.Success) TempData["ErrorMessage"] = result.ErrorMessage;
        else TempData["SuccessMessage"] = "Booking completed!";

        return RedirectToLocalReferer();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unbook(string id)
    {
        if (string.IsNullOrEmpty(id)) return BadRequest();
        var userId = userManager.GetUserId(User);
        var result = await gymClassService.UnbookClassAsync(userId!, id);

        if (result.Success) TempData["SuccessMessage"] = "Session cancelled!";
        else TempData["ErrorMessage"] = result.ErrorMessage;

        return RedirectToLocalReferer();
    }

    [Authorize(Roles = "Admin,admin")]
    [HttpGet]
    public IActionResult Create() => View();

    [Authorize(Roles = "Admin,admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GymClass model)
    {
        model.Id = Guid.NewGuid().ToString();
        ModelState.Remove("Id");

        if (!ModelState.IsValid) return View(model);

        var result = await gymClassService.CreateClassAsync(model);
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Class created successfully!";
            return RedirectToAction(nameof(Classes));
        }

        ModelState.AddModelError("", "Could not save to database.");
        return View(model);
    }

    [Authorize(Roles = "Admin,admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await gymClassService.DeleteClassAsync(id);
        if (result.Success) TempData["SuccessMessage"] = "Class deleted.";
        else TempData["ErrorMessage"] = "Could not delete class.";

        return RedirectToAction(nameof(Classes));
    }

    private IActionResult RedirectToLocalReferer()
    {
        var referer = Request.Headers["Referer"].ToString();
        if (!string.IsNullOrEmpty(referer) && Url.IsLocalUrl(referer))
        {
            return Redirect(referer);
        }
        return RedirectToAction(nameof(Classes));
    }
}