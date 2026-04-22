using Domain.Aggregates.Members;
using Domain.Persistence.Repositories;

namespace Domain.Abstractions.Repositories.Members;

public interface IMemberRepository : IRepositoryBase<Member, Guid>
{
    Task<Member?> GetMemberByUserIdAsync(string userId, CancellationToken ct = default);
}
