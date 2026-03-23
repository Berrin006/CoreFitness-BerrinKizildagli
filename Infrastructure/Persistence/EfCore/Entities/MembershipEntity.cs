namespace Infrastructure.Persistence.EfCore.Entities;

public class MembershipEntity
{
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string PlanName { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
}
