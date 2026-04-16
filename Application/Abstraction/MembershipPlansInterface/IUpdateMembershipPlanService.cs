using Application.Common.Results;
using Application.MembershipPlans.Inputs;
using Application.MembershipPlans.Outputs;

namespace Application.Abstraction.MembershipPlansInterface;

public interface IUpdateMembershipPlanService
{
    Task<Result<MembershipPlanOutput>> ExecuteAsync(UpdateMembershipPlanInput input, CancellationToken ct = default);
}
