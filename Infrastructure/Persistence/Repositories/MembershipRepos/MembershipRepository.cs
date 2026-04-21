using Domain.Abstractions.Repositories.Memberships;
using Domain.Aggregates.Memberships;
using Infrastructure.Entities.Membership;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.MembershipRepos;

public class MembershipRepository(DataContext context) : RepositoryBase<Membership, Guid, MembershipEntity, DataContext>(context), IMembershipRepository
{
    protected override Guid GetId(Membership model)
    {
        return model.Id;
    }

    public async Task<Membership?> GetByMemberIdAsync(Guid memberId, CancellationToken ct = default)
    {
        var membership = await context.Memberships.FirstOrDefaultAsync(x => x.MemberId == memberId, ct);
        
        if (membership == null)
            return null;

        var result = ToDomainModel(membership);

        return result;
    }

    public async Task<bool> RemoveByMemberIdAsync(Guid memberId, CancellationToken ct = default)
    {
        var membership = await context.Memberships.FirstOrDefaultAsync(x => x.MemberId == memberId, ct);

        if (membership == null)
            return false; 

        Set.Remove(membership);
        return true;
    }

    protected override void UpdateEntity(MembershipEntity entity, Membership model)
    {
        entity.StartDate = model.StartDate;

    }

    protected override Membership ToDomainModel(MembershipEntity entity)
    {
        var model = Membership.Rehydrate
            (
                entity.Id,
                entity.MemberId,
                entity.MembershipPlanId,
                entity.StartDate,
                entity.EndDate
            );

        return model;
    }

    protected override MembershipEntity ToEntity(Membership model)
    {
        var entity = new MembershipEntity
            (
                model.Id,
                model.MemberId,
                model.MembershipPlanId,
                model.StartDate,
                model.EndDate,
                model.UpdatedAt,
                model.CancelledAt
            );

        return entity;
    }
}
