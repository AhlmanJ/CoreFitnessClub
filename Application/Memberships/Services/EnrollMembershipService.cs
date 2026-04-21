
/*
 * Here i have taken support from AI to guide me through the process of creating a membership.
 * I have created the code and when i have encountered problems, AI has been my teacher and explained what i am doing wrong and how to approach the problem.
 */

using Application.Abstraction;
using Application.Abstraction.MembershipInterface;
using Application.Common.Results;
using Application.Memberships.Inputs;
using Application.Memberships.Outputs;
using Domain.Abstractions.Repositories.Members;
using Domain.Abstractions.Repositories.MembershipPlans;
using Domain.Abstractions.Repositories.Memberships;
using Domain.Aggregates.Memberships;

namespace Application.Memberships.Services;

public class EnrollMembershipService(IMembershipRepository membershipRepository, IMemberRepository memberRepository, IMembershipPlanRepository membershipPlanRepository, IUnitOfWork _unitOfWork) : IEnrollMembershipService
{
    public async Task<Result<MembershipResponseOutput>> ExecuteAsync(EnrollMembershipInput input, CancellationToken ct = default)
    {
        try
        {
            if (input is null)
                return Result<MembershipResponseOutput>.BadRequest("input must be provided.");

            var member = await memberRepository.GetMemberByUserIdAsync(input.UserId);

            if (member is null)
                return Result<MembershipResponseOutput>.BadRequest("Could not find the member.");

            if (member.FirstName is null)
                return Result<MembershipResponseOutput>.BadRequest("First name not found.");

            if (member.LastName is null)
                return Result<MembershipResponseOutput>.BadRequest("Last name not found.");

            var memberId = member.Id;

            var existingMembership = await membershipRepository.GetByMemberIdAsync(memberId, ct);
            if (existingMembership != null)
                return Result<MembershipResponseOutput>.BadRequest("You already have an existing membership.");

            var plan = await membershipPlanRepository.GetByIdAsync(input.MembershipPlanId, ct);

            if (plan is null)
                return Result<MembershipResponseOutput>.BadRequest("Plan not found.");

            var start = DateTimeOffset.UtcNow;
            var end = start.AddDays(plan.ValidDays);

            var newMembership = Membership.Create
                (
                    memberId,
                    input.MembershipPlanId,
                    start,
                    end
                );

            await membershipRepository.AddAsync(newMembership, ct);

            await _unitOfWork.CommitAsync(ct);

            var result = new MembershipResponseOutput
                (
                    member.Id,
                    member.FirstName,
                    member.LastName,
                    plan.Name,
                    plan.Price,
                    start,
                    end
                );

            return Result<MembershipResponseOutput>.Ok(result);
        }
        catch (Exception ex) 
        {
            return Result<MembershipResponseOutput>.InternalServerError(ex.Message);
        }
    }
}
