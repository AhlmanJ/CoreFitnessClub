using Application.Abstraction.MembershipPlansInterface;
using Application.Common.Results;
using Domain.Abstractions.Repositories.MembershipPlans;

namespace Application.MembershipPlans.Services;

public class DeleteMembershipPlanService(IMembershipPlanRepository membershipPlanRepository) : IDeleteMembershipPlanService
{
    public async Task<Result<string?>> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
            return Result<string?>.BadRequest("Id cannot be empty");

        try 
        { 
            var planToDelete = await membershipPlanRepository.GetByIdAsync(id, ct);
            if (planToDelete == null)
                return Result<string?>.NotFound("Could not fint the membership plan.");

            await membershipPlanRepository.RemoveAsync(planToDelete);
            return Result<string?>.Ok("The membership plan was deleted");
        }
        catch (Exception ex) 
        {
            return Result<string?>.InternalServerError(ex.Message);
        }
    }
}
