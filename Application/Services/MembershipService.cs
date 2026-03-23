using Application.Common.Results;
using Domain.Abstractions.Repositories;
using Domain.Aggregates.Memberships;

namespace Application.Services;

public class MembershipService(IMembershipRepository membershipRepository) : IMembershipService
{
    public async Task<Result<bool>> CreateMembershipAsync(string userId, string planName)
    {
        try
        {
            var existing = await membershipRepository.GetByUserIdAsync(userId);
            if (existing != null && existing.IsActive)
            {
                return Result<bool>.Conflict("Du har redan ett aktivt medlemskap.");
            }

            var membership = new Membership
            {
                UserId = userId,
                PlanName = planName,
                StartDate = DateTime.Now,
                IsActive = true
            };

            await membershipRepository.AddAsync(membership);

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }

    public async Task<Membership?> GetUserMembershipAsync(string userId)
    {
        return await membershipRepository.GetByUserIdAsync(userId);
    }
}