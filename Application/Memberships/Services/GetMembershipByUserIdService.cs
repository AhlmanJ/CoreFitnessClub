using Application.Abstraction.MembershipInterface;
using Application.Abstraction.MembershipReadInterface;
using Application.Common.Results;
using Application.Memberships.Outputs;
using Domain.Abstractions.Repositories.Members;

namespace Application.Memberships.Services;

public class GetMembershipByUserIdService(IMembershipQueryService membershipQueryService, IMemberRepository memberRepository) : IGetMembershipByUserIdService
{
    public async Task<Result<MembershipResponseOutput?>> ExecuteAsync(string userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Result<MembershipResponseOutput?>.BadRequest("User ID must be provided.");

        try 
        {
            var member = await memberRepository.GetMemberByUserIdAsync(userId);
            if (member is null)
                return Result<MembershipResponseOutput?>.NotFound("Member not found.");

            var output = await membershipQueryService.GetMembershipByMemberIdAsync(member.Id, ct);

            if (output == null)
                return Result<MembershipResponseOutput?>.BadRequest("Could not find an active membership for the requested member.");

            return Result<MembershipResponseOutput?>.Ok(output);
        }
        catch (Exception ex)
        {
            return Result<MembershipResponseOutput?>.InternalServerError(ex.Message);
        }
    }
}
