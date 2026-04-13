using Application.Common.Results;
using Application.Members.Inputs;
using Application.Members.Outputs;

namespace Application.Abstraction.MembersInterface;

public interface IUpdateMemberProfileService
{
    Task<Result<MemberProfileOutput>> ExecuteAsync(UpdateMemberProfileInput input, CancellationToken ct = default);
}