namespace Domain.Aggregates.Memberships;

public class Membership
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); 
    public string UserId { get; set; } = null!;
    public string PlanName { get; set; } = null!; 
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime? EndDate { get; set; } 
    public bool IsActive { get; set; } = true;
}