
/*
 * I have taken help from both the lectures at school and chatGPT to be able to create this service.
 * I have recived a lot of guidance from AI to be able to understand how to implement MVC, services and input/outputs as it was very difficult to understand.
 */

using Application.Abstraction.MembershipPlansInterface;
using Application.Common.Results;
using Application.MembershipPlans.Outputs;
using Domain.Abstractions.Repositories.MembershipPlans;

namespace Application.MembershipPlans.Services;

public class GetAllMembershipPlansService(IMembershipPlanRepository membershipPlanRepository) : IGetAllMembershipPlansService
{
    public async Task<Result<List<MembershipPlanOutput>>> ExecuteAsync(CancellationToken ct = default)
    {
        try
        {
            var membershipPlans = await membershipPlanRepository.GetAllAsync(ct);
            if (membershipPlans == null)
                return Result<List<MembershipPlanOutput>>.Ok(new List<MembershipPlanOutput>());

            var output = membershipPlans.Select(plan => new MembershipPlanOutput( 
                plan.Id,
                plan.Name,
                plan.Description,
                plan.ListItem1,
                plan.ListItem2,
                plan.ListItem3,
                plan.Price,
                plan.ValidDays
            )).ToList();

            return Result<List<MembershipPlanOutput>>.Ok(output);
        }
        catch (Exception ex)
        {
            return Result<List<MembershipPlanOutput>>.InternalServerError(ex.Message);
        }
    }
}
