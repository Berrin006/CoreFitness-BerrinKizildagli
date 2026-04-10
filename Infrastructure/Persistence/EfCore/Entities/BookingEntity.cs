using System.ComponentModel.DataAnnotations;
using Domain.Aggregates.GymClasses;

namespace Infrastructure.Persistence.EfCore.Entities;

public class BookingEntity
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public string GymClassId { get; set; } = null!; 
    public DateTime BookedAt { get; set; }

    public virtual GymClass GymClass { get; set; } = null!;
}