using Application.Abstraction;
using Application.Abstraction.MembershipPlansInterface;
using Application.Common.Results;
using Application.MembershipPlans.Inputs;
using Application.MembershipPlans.Outputs;
using Domain.Abstractions.Repositories.MembershipPlans;
using Domain.Aggregates.MembershipPlan;

namespace Application.MembershipPlans.Services;

public class CreateMembershipPlanService(IMembershipPlanRepository membershipPlanRepository, IUnitOfWork _unitOfWork) : ICreateMembershipPlanService
{
    public async Task<Result<MembershipPlanOutput>> ExecuteAsync(CreateMembershipPlanInput input, CancellationToken ct = default)
    {
        try
        {
            if (input is null)
                return Result<MembershipPlanOutput>.BadRequest("The input field was empty.");

            var existingPlans = await membershipPlanRepository.GetAllAsync(ct);
            if (existingPlans.Count() >= 2)
                return Result<MembershipPlanOutput>.BadRequest("You can only have two membership plans at the same time.");

            var membershipPlan = Domain.Aggregates.MembershipPlan.MembershipPlan.Create
                (
                    input.Name,
                    input.Description,
                    input.Price,
                    input.ValidDays
                );

            await membershipPlanRepository.AddAsync(membershipPlan, ct);
            await _unitOfWork.CommitAsync(ct);

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