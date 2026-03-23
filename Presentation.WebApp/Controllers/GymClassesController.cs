using Application.Services;
using Domain.Aggregates.GymClasses;
using Infrastructure.Persistence.EfCore.Contexts;
using Infrastructure.Persistence.EfCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.WebApp.Attributes.MenuNavigation;
using System.Security.Claims;

namespace Presentation.WebApp.Controllers;

public class GymClassesController(
    IGymClassService gymClassService,
    UserManager<IdentityUser> userManager,
    DataContext context) : Controller
{
    private readonly DataContext _context = context;

    [MenuItem(Title = "Gym Classes", Order = 2)]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await gymClassService.GetAllAsync();
        return View(result.Value ?? Enumerable.Empty<GymClass>());
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Book(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await gymClassService.BookClassAsync(userId!, id);

        if (!result.Success) TempData["ErrorMessage"] = result.ErrorMessage;
        else TempData["SuccessMessage"] = "Booking completed!";

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Unbook(string id)
    {
        var userId = userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId)) return Challenge();

        var result = await gymClassService.UnbookClassAsync(userId, id);
        if (result.Success) TempData["SuccessMessage"] = "Session cancelled!";
        else TempData["ErrorMessage"] = result.ErrorMessage;

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GymClass model)
    {
        if (!ModelState.IsValid) return View(model);

        var entity = new GymClassEntity
        {
            Id = Guid.NewGuid().ToString(),
            Name = model.Name,
            Instructor = model.Instructor,
            StartTime = model.StartTime
        };

        await _context.GymClasses.AddAsync(entity);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Class created successfully!";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var gymClass = await _context.GymClasses.FindAsync(id);
        if (gymClass != null)
        {
            _context.GymClasses.Remove(gymClass);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Class deleted.";
        }
        return RedirectToAction(nameof(Index));
    }
}