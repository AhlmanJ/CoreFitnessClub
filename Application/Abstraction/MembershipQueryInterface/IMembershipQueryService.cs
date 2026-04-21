using Application.Memberships.Outputs;

namespace Application.Abstraction.MembershipReadInterface;

public interface IMembershipQueryService
{
    Task<MembershipResponseOutput?> GetMembershipByMemberIdAsync(Guid memberId, CancellationToken ct = default);
    Task<List<MembershipResponseOutput>> GetAllMembershipsAsync(CancellationToken ct = default);
}
