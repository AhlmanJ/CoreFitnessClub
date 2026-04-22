using Domain.Aggregates.Memberships;
using Domain.Persistence.Repositories;

namespace Domain.Abstractions.Repositories.Memberships;

public interface IMembershipRepository : IRepositoryBase<Membership, Guid>
{
    Task<Membership?> GetByMemberIdAsync(Guid memberId, CancellationToken ct = default);
    Task<bool> RemoveByMemberIdAsync(Guid memberId, CancellationToken ct = default);
}
