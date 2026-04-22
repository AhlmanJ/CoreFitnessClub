using Application.Common.Results;
using Application.Memberships.Outputs;

namespace Application.Abstraction.MembershipInterface;

public interface IGetAllMembershipsService
{
    Task<Result<List<MembershipResponseOutput>>> ExecuteAsync(CancellationToken ct = default);
}
