using Application.Common.Results;

namespace Application.Abstraction.MembershipPlansInterface;

public interface IDeleteMembershipPlanService
{
    Task<Result<string?>> ExecuteAsync(Guid id, CancellationToken ct = default);
}
