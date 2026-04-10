using Microsoft.AspNetCore.Identity;

namespace Domain.Aggregates.GymClasses;

public class Booking : BaseEntity 
{
    public string UserId { get; set; } = null!;
    public IdentityUser User { get; set; } = null!;

    public int GymClassId { get; set; }
    public GymClass GymClass { get; set; } = null!;

    public DateTime BookedAt { get; set; } = DateTime.UtcNow;
}