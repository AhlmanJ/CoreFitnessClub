using Domain.Aggregates.Members;
using Domain.Persistence.Repositories;
using System.Security.Cryptography;

namespace Domain.Abstractions.Repositories.Members;

public interface IMemberRepository : IRepositoryBase<Member, Guid>
{
    Task<Member?> GetMemberByUserIdAsync(string userId, CancellationToken ct = default);
    string GetUserId(Member model);
}
