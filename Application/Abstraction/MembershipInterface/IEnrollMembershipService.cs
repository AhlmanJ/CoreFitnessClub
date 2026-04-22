using Application.Common.Results;
using Application.Memberships.Inputs;
using Application.Memberships.Outputs;

namespace Application.Abstraction.MembershipInterface;

public interface IEnrollMembershipService
{
    Task<Result<MembershipResponseOutput>> ExecuteAsync(EnrollMembershipInput input, CancellationToken ct = default);
}
