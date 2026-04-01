using Application.Common.Results;
using Application.Members.Inputs;

namespace Application.Abstraction.MembersInterface;

public interface ISignInMemberService
{
    Task<Result<string?>> ExecuteAsync(SignInInput input, CancellationToken ct = default);
}