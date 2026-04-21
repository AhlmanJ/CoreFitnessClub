using Application.Common.Results;

namespace Application.Abstraction.MembershipInterface;

public interface IDeleteMembershipByMemberIdService
{
    Task<Result<string?>> ExecuteAsync(Guid memberId, CancellationToken ct = default);
}
