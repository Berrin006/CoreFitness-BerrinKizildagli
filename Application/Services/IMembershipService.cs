using Application.Common.Results;
using Domain.Aggregates.Memberships;

namespace Application.Services;

public interface IMembershipService
{
    Task<Result<bool>> CreateMembershipAsync(string userId, string planName);
    Task<Membership?> GetUserMembershipAsync(string userId);
}