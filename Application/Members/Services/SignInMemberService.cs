using Application.Abstraction;
using Application.Abstraction.MembersInterface;
using Application.Common.Results;
using Application.Members.Inputs;
using Application.Members.Outputs;

namespace Application.Members.Services;

public class SignInMemberService(IIdentityService identityService) : ISignInMemberService
{
    public async Task<Result<string?>> ExecuteAsync(SignInInput input, CancellationToken ct = default)
    {
        try
        {
            if (input is null)
                return Result<string?>.BadRequest("The input field was empty.");


            var result = await identityService.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, ct);
            return !result.Success ? Result<string?>.Unauthorized("invalid email or password") : Result<string?>.Ok("login succeeded");
        }
        catch (Exception ex)
        {
            return Result<string?>.InternalServerError(ex.Message);
        }
    }
}
