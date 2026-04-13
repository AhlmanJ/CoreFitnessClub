using Application.Common.Results;
using Application.Members.Inputs;
using Application.Members.Outputs;

namespace Application.Abstraction.MembersInterface;

public interface IRegisterMemberAccountService
{
    Task<Result<MemberResponseOutput>> ExecuteAsync(RegisterMemberAccountInput input, CancellationToken ct = default);
}