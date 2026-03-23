namespace Infrastructure.Persistence.EfCore.Entities;

public class GymClassEntity
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public string Instructor { get; set; } = null!;
}