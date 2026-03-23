using Domain.Abstractions.Repositories;
using Domain.Aggregates.Memberships;
using Infrastructure.Persistence.EfCore.Contexts;
using Infrastructure.Persistence.EfCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EfCore.Repositories;

public class MembershipRepository(DataContext context)
    : RepositoryBase<Membership, string, MembershipEntity, DataContext>(context), IMembershipRepository
{
    protected override Membership ToModel(MembershipEntity entity)
    {
        return new Membership
        {
            Id = entity.Id,
            UserId = entity.UserId,
            PlanName = entity.PlanName,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            IsActive = entity.IsActive
        };
    }

    protected override MembershipEntity ToEntity(Membership model)
    {
        return new MembershipEntity
        {
            Id = model.Id,
            UserId = model.UserId,
            PlanName = model.PlanName,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            IsActive = model.IsActive
        };
    }

    protected override void ApplyUpdates(Membership model, MembershipEntity entity)
    {
        entity.PlanName = model.PlanName;
        entity.StartDate = model.StartDate;
        entity.EndDate = model.EndDate;
        entity.IsActive = model.IsActive;
    }

    protected override string GetId(Membership model) => model.Id;

    public async Task<Membership?> GetByUserIdAsync(string userId)
    {
        var entity = await _context.Memberships
            .FirstOrDefaultAsync(m => m.UserId == userId && m.IsActive);

        return entity != null ? ToModel(entity) : null;
    }
}