using Application.Abstraction.MembersInterface;
using Application.Common.Results;
using Application.Members.Inputs;
using Domain.Abstractions.Repositories.Members;
using Domain.Aggregates.Members;

namespace Application.Members.Services;

public class UpdateMemberProfileService(IMemberRepository memberRepository) : IUpdateMemberProfileService
{
    public async Task<Result<Member>> ExecuteAsync(UpdateMemberProfileInput input, CancellationToken ct = default)
    {
        try
        {
            if (input is null)
                throw new ArgumentException("input must be provided.");

            var member = await memberRepository.GetMemberByUserIdAsync(input.UserId, ct);
            if (member is null)
                return Result<Member>.NotFound($"Member with user id {input.UserId} was not found.");

            member.UpdateInformation(input.FirstName, input.LastName, input.PhoneNumber, input.ProfileImageUrl);
            var result = await memberRepository.UpdateAsync(member, ct);

            return !result
                ? Result<Member>.InternalServerError($"Member with User Id {input.UserId} was not updated.") : Result<Member>.Ok(member);
        }
        catch (Exception ex)
        {
            return Result<Member>.InternalServerError(ex.Message);
        }
    }
}