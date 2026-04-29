using Application.Abstraction.MembersQueryInterface;
using Application.Members.Outputs;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.QueryServices;

public class MemberQueryService(DataContext context) : IMemberQueryService
{
    public async Task<List<MemberProfileOutput>> GetAllMembersAsync(CancellationToken ct = default)
    {
        return await context.Members
            .Select(m => new MemberProfileOutput
            (
               m.Id,
               m.UserId,
               m.FirstName,
               m.LastName,
               m.PhoneNumber,
               m.ProfileImageUrl,
               m.CreatedAt,
               m.ModifiedAt
            )).ToListAsync();
    }
}
