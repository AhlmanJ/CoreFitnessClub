using Application.Common.Results;
using Application.Memberships.Outputs;

namespace Application.Abstraction.MembershipInterface;

public interface IGetMembershipByUserIdService
{
    Task<Result<MembershipResponseOutput?>> ExecuteAsync(String UserId, CancellationToken ct = default);
}
