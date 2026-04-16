using Application.Common.Results;
using Application.MembershipPlans.Outputs;

namespace Application.Abstraction.MembershipPlansInterface;

public interface IGetMembershipPlanService
{
    Task<Result<MembershipPlanOutput>> ExecuteAsync(Guid id, CancellationToken ct = default);
}
