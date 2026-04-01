using Application.Common.Results;
using Domain.Aggregates.Members;

namespace Application.Abstraction.MembersInterface;

public interface IGetMemberProfileService
{
    Task<Result<Member>> ExecuteAsync(string userId, CancellationToken ct = default);
}