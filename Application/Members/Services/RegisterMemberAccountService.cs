using Application.Abstraction;
using Application.Abstraction.MembersInterface;
using Application.Common.Results;
using Application.Members.Inputs;
using Domain.Abstractions.Repositories.Members;
using Domain.Aggregates.Members;

namespace Application.Members.Services;

public class RegisterMemberAccountService(IIdentityService identityService, IMemberRepository memberRepository) : IRegisterMemberAccountService
{
    public async Task<Result<string?>> ExecuteAsync(RegisterMemberAccountInput input, CancellationToken ct = default)
    {
        try
        {
            if (input is null)
                throw new ArgumentException("The input field was empty.");

            var createUserResult = await identityService.CreateUserAsync(input.Email, input.Password, ct);
            if (!createUserResult.Success || string.IsNullOrWhiteSpace(createUserResult.Value))
                return Result<string?>.InternalServerError(createUserResult?.ErrorMessage ?? "Unable to create user account");

            var member = Member.Create(createUserResult.Value);

            await memberRepository.AddAsync(member, ct);
            return Result<string?>.Ok(member.UserId);
        }
        catch (Exception ex)
        {
            return Result<string?>.InternalServerError(ex.Message);
        }
    }
}
