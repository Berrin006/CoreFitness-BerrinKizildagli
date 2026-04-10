using Domain.Aggregates.Memberships;

public interface IMembershipService
{
    Task<bool> CreateMembershipAsync(string userId, string planName);
    Task<Membership?> GetMembershipByUserIdAsync(string userId);

    Task<bool> CancelMembershipAsync(string userId);
}