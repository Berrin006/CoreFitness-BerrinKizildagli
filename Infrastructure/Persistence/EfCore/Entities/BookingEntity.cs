namespace Infrastructure.Persistence.EfCore.Entities;

public class BookingEntity
{
    public string UserId { get; set; } = null!;

    public string GymClassId { get; set; } = null!;

    public DateTime BookedAt { get; set; } = DateTime.UtcNow;

    public virtual GymClassEntity GymClass { get; set; } = null!;
}