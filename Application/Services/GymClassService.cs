using Application.Common.Results;
using Domain.Abstractions.Repositories;
using Domain.Aggregates.GymClasses;
using Infrastructure.Persistence.EfCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class GymClassService(IGymClassRepository repository, DataContext context) : IGymClassService
{
    private readonly IGymClassRepository _repository = repository;
    private readonly DataContext _context = context;

    public async Task<Result<IEnumerable<GymClass>>> GetAllAsync()
    {
        var classes = await _repository.GetAllAsync();
        return Result<IEnumerable<GymClass>>.Ok(classes);
    }

    public async Task<Result> BookClassAsync(string userId, string gymClassId)
    {
        try
        {
            var booking = new BookingEntity
            {
                UserId = userId,
                GymClassId = gymClassId,
                BookedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Error("You are already booked for this session.");
        }
    }

    public async Task<Result> UnbookClassAsync(string userId, string gymClassId)
    {
        try
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync<BookingEntity>(b => b.UserId == userId && b.GymClassId == gymClassId);

            if (booking == null)
            {
                return Result.Error("No booking was found to cancel.");
            }

            _context.Bookings.Remove(booking);

            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Error("An error occurred during cancellation.");
        }
    }
}