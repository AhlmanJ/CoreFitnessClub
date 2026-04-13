using Application.Abstraction.MembersInterface;
using Application.Common.Results;
using Application.Members.Inputs;
using Application.Members.Outputs;
using Domain.Abstractions.Repositories.Members;

namespace Application.Members.Services;

public class UpdateMemberProfileService(IMemberRepository memberRepository) : IUpdateMemberProfileService
{
    public async Task<Result<MemberProfileOutput>> ExecuteAsync(UpdateMemberProfileInput input, CancellationToken ct = default)
    {
        try
        {
            if (input is null)
                throw new ArgumentException("input must be provided.");

            var member = await memberRepository.GetMemberByUserIdAsync(input.UserId, ct);
            if (member is null)
                return Result<MemberProfileOutput>.NotFound($"Member with user id {input.UserId} was not found.");

            member.UpdateInformation(input.FirstName, input.LastName, input.PhoneNumber, input.ProfileImageUrl);
            var result = await memberRepository.UpdateAsync(member, ct);

            return !result
                ? Result<MemberProfileOutput>.InternalServerError($"Member with User Id {input.UserId} was not updated.") : Result<MemberProfileOutput>.Ok
                ( new MemberProfileOutput (
                     member.Id,
                     member.UserId,
                     member.FirstName,
                     member.LastName,
                     member.PhoneNumber,
                     member.ProfileImageUrl,
                     member.CreatedAt,
                     member.ModifiedAt
                ));
        }
        catch (Exception ex)
        {
            return Result<MemberProfileOutput>.InternalServerError(ex.Message);
        }
    }
}