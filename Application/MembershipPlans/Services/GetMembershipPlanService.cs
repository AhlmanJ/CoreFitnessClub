using Application.Abstraction.MembershipPlansInterface;
using Application.Common.Results;
using Application.MembershipPlans.Outputs;
using Domain.Abstractions.Repositories.MembershipPlans;

namespace Application.MembershipPlans.Services;

public class GetMembershipPlanService(IMembershipPlanRepository membershipPlanRepository) : IGetMembershipPlanService
{
    public async Task<Result<MembershipPlanOutput>> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
            return Result<MembershipPlanOutput>.BadRequest("Id cannot be empty.");

            var membershipPlan = await membershipPlanRepository.GetByIdAsync(id,ct);
        if (membershipPlan == null)
            return Result<MembershipPlanOutput>.NotFound("Could not find the membership plan.");

        var membershipPlanOutput = new MembershipPlanOutput(
            membershipPlan.Id,
            membershipPlan.Name,
            membershipPlan.Description,
            membershipPlan.ListItem1,
            membershipPlan.ListItem2,
            membershipPlan.ListItem3,
            membershipPlan.Price,
            membershipPlan.ValidDays
            );

            return Result<MembershipPlanOutput>.Ok(membershipPlanOutput);
    }
}
