using Application.Common.Results;
using Application.Services;
using Domain.Abstractions.Repositories;
using Domain.Aggregates.GymClasses;
using Infrastructure.Data;
using Infrastructure.Persistence.EfCore.Contexts;
using Infrastructure.Persistence.EfCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class GymClassService(IGymClassRepository repository, ApplicationDbContext context) : IGymClassService
{
    private readonly IGymClassRepository _repository = repository;
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<GymClass>>> GetAllAsync()
    {
        var classes = await _repository.GetAllAsync();
        return Result<IEnumerable<GymClass>>.Ok(classes);
    }

    public async Task<Result> BookClassAsync(string userId, string gymClassId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(gymClassId))
            {
                return Result.Error("Användar-ID eller Klass-ID saknas.");
            }

            var booking = new BookingEntity
            {
                UserId = userId,
                GymClassId = gymClassId,
                BookedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);

            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows > 0)
            {
                return Result.Ok();
            }

            return Result.Error("Kunde inte spara bokningen i databasen.");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BOKNINGSFEL: {ex.Message}");
            return Result.Error($"Ett fel uppstod: {ex.Message}");
        }
    }

    public async Task<Result> UnbookClassAsync(string userId, string gymClassId)
    {
        try
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.GymClassId == gymClassId);

            if (booking == null) return Result.Error("No booking was found.");

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Error("An error occurred.");
        }
    }

    public async Task<Result> CreateClassAsync(GymClass model)
    {
        try
        {
            await _repository.AddAsync(model);
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Error("Failed to create class.");
        }
    }

    public async Task<Result> DeleteClassAsync(string id)
    {
        try
        {
            var classes = await _repository.GetAllAsync();
            var classToDelete = classes.FirstOrDefault(x => x.Id == id);

            if (classToDelete == null) return Result.Error("Not found.");

            await _repository.DeleteAsync(classToDelete);
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Error("Failed to delete.");
        }
    }

    public async Task<List<string>> GetBookedClassIdsForUserAsync(string userId)
    {
        var ids = await _context.Bookings
            .Where(b => b.UserId.Trim() == userId.Trim())
            .Select(b => b.GymClassId)
            .ToListAsync();

        return ids ?? new List<string>();
    }

    public async Task<IEnumerable<GymClass>> GetBookedClassesForUserAsync(string userId)
    {
        var bookings = await _context.Bookings
            .Include(b => b.GymClass)
            .Where(b => b.UserId == userId)
            .ToListAsync();

        return bookings.Select(b => new GymClass
        {
            Id = b.GymClass.Id,
            Name = b.GymClass.Name,
            Instructor = b.GymClass.Instructor,
            StartTime = b.GymClass.StartTime
        });
    }
}