using Application.Abstraction.MembersInterface;
using Application.Common.Results;
using Domain.Abstractions.Repositories.Members;
using Domain.Aggregates.Members;

namespace Application.Members.Services;

public class GetMemberProfileService(IMemberRepository memberRepository) : IGetMemberProfileService
{
    public async Task<Result<Member>> ExecuteAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
                return Result<Member>.BadRequest("UserId must be provided.");

            var member = await memberRepository.GetMemberByUserIdAsync(userId, ct);
            return member is null ? Result<Member>.NotFound($"Member with User Id {userId} was not found") : Result<Member>.Ok(member);
        }
        catch (Exception ex)
        {
            return Result<Member>.InternalServerError(ex.Message);
        }
    }
}
