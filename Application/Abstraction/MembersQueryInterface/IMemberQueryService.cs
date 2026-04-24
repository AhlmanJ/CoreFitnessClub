using Application.Members.Outputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstraction.MembersQueryInterface;

public interface IMemberQueryService
{
    Task<List<MemberProfileOutput>> GetAllMembersAsync(CancellationToken ct = default);
}
