using Application.Common.Results;
using Application.MembershipPlans.Outputs;

namespace Application.Abstraction.MembershipPlansInterface;

public interface IGetAllMembershipPlansService
{
    Task<Result<List<MembershipPlanOutput>>> ExecuteAsync(CancellationToken ct = default);
}
