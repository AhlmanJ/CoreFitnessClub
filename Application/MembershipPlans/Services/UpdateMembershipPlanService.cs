using Application.Abstraction.MembershipPlansInterface;
using Application.Common.Results;
using Application.MembershipPlans.Inputs;
using Application.MembershipPlans.Outputs;
using Domain.Abstractions.Repositories.MembershipPlans;

namespace Application.MembershipPlans.Services;

public class UpdateMembershipPlanService(IMembershipPlanRepository membershipPlanRepository) : IUpdateMembershipPlanService
{
    public async Task<Result<MembershipPlanOutput>> ExecuteAsync(UpdateMembershipPlanInput input, CancellationToken ct = default)
    {
        try 
        {
            if (input == null)
                throw new ArgumentException("Input must be provided");

            var membershipPlan = await membershipPlanRepository.GetByIdAsync(input.Id, ct);
            if (membershipPlan == null)
                return Result<MembershipPlanOutput>.NotFound($"The plan with the Id {input.Id} was not found");

            membershipPlan.UpdateMembershipPlan(input.Name, input.Description, input.Price, input.ValidDays);

            var result = await membershipPlanRepository.UpdateAsync(membershipPlan, ct);

           return !result
                ? Result<MembershipPlanOutput>.InternalServerError($"Membership plan with Id {input.Id} was not updated.") : Result<MembershipPlanOutput>.Ok
                ( new MembershipPlanOutput(
                    membershipPlan.Id,
                    membershipPlan.Name,
                    membershipPlan.Description,
                    membershipPlan.Price,
                    membershipPlan.ValidDays
                ));
        }
        catch (Exception ex)
        {
            return Result<MembershipPlanOutput>.InternalServerError(ex.Message);
        }
    }
}
