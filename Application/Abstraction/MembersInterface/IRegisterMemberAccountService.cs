using Application.Common.Results;
using Application.Members.Inputs;

namespace Application.Abstraction.MembersInterface;

public interface IRegisterMemberAccountService
{
    Task<Result<string?>> ExecuteAsync(RegisterMemberAccountInput input, CancellationToken ct = default);
}