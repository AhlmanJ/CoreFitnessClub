using Application.Common.Results;
using Application.MembershipPlans.Inputs;
using Application.MembershipPlans.Outputs;

namespace Application.Abstraction.MembershipPlansInterface;

public interface ICreateMembershipPlanService
{
    Task<Result<MembershipPlanOutput>> ExecuteAsync(CreateMembershipPlanInput input, CancellationToken ct = default);
}
