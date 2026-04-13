using Domain.Entities.Members;
using Domain.Entities.Membership.MembershipPlan;

namespace Domain.Entities.Membership;

public class MembershipEntity
{
    public Guid Id { get; private set; }
    public Guid MemberId { get; private set; }
    public virtual MemberEntity Member { get; private set; } = null!;

    public Guid MembershipPlanId { get; private set; }
    public virtual MembershipPlanEntity MembershipPlan { get; private set; } = null!;


    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? CancelledAt { get; private set; }
}
