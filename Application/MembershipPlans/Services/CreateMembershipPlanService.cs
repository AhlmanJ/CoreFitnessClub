using Application.Abstraction.MembershipPlansInterface;
using Application.Common.Results;
using Application.MembershipPlans.Inputs;
using Application.MembershipPlans.Outputs;
using Domain.Abstractions.Repositories.MembershipPlans;
using Domain.Aggregates.MembershipPlan;

namespace Application.MembershipPlans.Services;

public class CreateMembershipPlanService(IMembershipPlanRepository membershipPlanRepository) : ICreateMembershipPlanService
{
    public async Task<Result<MembershipPlanOutput>> ExecuteAsync(CreateMembershipPlanInput input, CancellationToken ct = default)
    {
        try
        {
            if (input is null)
                return Result<MembershipPlanOutput>.BadRequest("The input field was empty.");

            var membershipPlans = await membershipPlanRepository.GetAllAsync(ct);
            if (membershipPlans.Count() >= 2)
                return Result<MembershipPlanOutput>.BadRequest("You can only have two membership plans at the same time.");

            var newGuid = Guid.NewGuid();

            var membershipPlan = MembershipPlan.Create
                (
                    newGuid,
                    input.Name,
                    input.Description,
                    input.Price,
                    input.ValidDays
                );

            await membershipPlanRepository.AddAsync(membershipPlan, ct);

            var result = new MembershipPlanOutput
                (
                    membershipPlan.Id,
                    membershipPlan.Name,
                    membershipPlan.Description,
                    membershipPlan.Price,
                    membershipPlan.ValidDays
                );

            return Result<MembershipPlanOutput>.Ok(result);
        }
        catch (Exception ex)
        {
            return Result<MembershipPlanOutput>.InternalServerError(ex.Message);
        }
    }
}