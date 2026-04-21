using Application.Abstraction.MembershipReadInterface;
using Application.Memberships.Outputs;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.QueryServices;

public class MembershipQueryService(DataContext context) : IMembershipQueryService
{
    public async Task<MembershipResponseOutput?> GetMembershipByMemberIdAsync(Guid memberId, CancellationToken ct = default)
    {
        return await context.Memberships
            .Where(x => x.MemberId == memberId)
            .Select(m => new MembershipResponseOutput
            (
                m.Members.Id,
                m.Members.FirstName ?? "",
                m.Members.LastName ?? "",
                m.MembershipPlan.Name,
                m.MembershipPlan.Price,
                m.StartDate,
                m.EndDate
            )).FirstOrDefaultAsync(ct);
    }

    public async Task<List<MembershipResponseOutput>> GetAllMembershipsAsync(CancellationToken ct = default)
    {
        return await context.Memberships
            .Select(m => new MembershipResponseOutput
            (
                m.Members.Id,
                m.Members.FirstName ?? "",
                m.Members.LastName ?? "",
                m.MembershipPlan.Name,
                m.MembershipPlan.Price,
                m.StartDate,
                m.EndDate
            )).ToListAsync();
    }
}