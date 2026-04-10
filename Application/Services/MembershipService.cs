using Application.Common.Results;
using Domain.Abstractions.Repositories;
using Domain.Aggregates.Memberships;

namespace Application.Services;

public class MembershipService : IMembershipService
{
    private readonly IMembershipRepository _repository;

    public MembershipService(IMembershipRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> CreateMembershipAsync(string userId, string planName)
    {
        var existingMembership = await _repository.GetByUserIdAsync(userId);

        if (existingMembership != null)
        {
            existingMembership.PlanName = planName;
            existingMembership.StartDate = DateTime.Now; 
            existingMembership.IsActive = true;

            await _repository.UpdateAsync(existingMembership);
        }
        else
        {
            var membership = new Membership
            {
                UserId = userId,
                PlanName = planName,
                StartDate = DateTime.Now,
                IsActive = true
            };

            await _repository.AddAsync(membership);
        }

        return true;
    }

    public async Task<Membership?> GetMembershipByUserIdAsync(string userId)
    {
        return await _repository.GetByUserIdAsync(userId);
    }

    public async Task<bool> CancelMembershipAsync(string userId)
    {
        var membership = await _repository.GetByUserIdAsync(userId);
        if (membership == null) return false;

        
        await _repository.DeleteAsync(membership);

        return true;
    }
}