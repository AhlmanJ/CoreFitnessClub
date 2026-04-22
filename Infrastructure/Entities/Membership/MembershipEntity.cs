using Infrastructure.Entities.Members;
using Infrastructure.Entities.MembershipPlan;

namespace Infrastructure.Entities.Membership;

public class MembershipEntity
{
    public Guid Id { get; internal set; }
    public Guid MemberId { get; internal set; }
    public virtual MemberEntity Members { get; internal set; } = null!;

    public Guid MembershipPlanId { get; internal set; }
    public virtual MembershipPlanEntity MembershipPlan { get; internal set; } = null!;

    public DateTimeOffset StartDate { get; internal set; }
    public DateTimeOffset EndDate { get; internal set; }

    public DateTimeOffset? UpdatedAt { get; internal set; }
    public DateTimeOffset? CancelledAt { get; internal set; }

    public byte[] RowVersion { get; private set; } = null!;

    private MembershipEntity() 
    {
    
    }

    public MembershipEntity(Guid id, Guid memberId, Guid membershipPlanId, DateTimeOffset startDate, DateTimeOffset endDate, DateTimeOffset? updatedAt, DateTimeOffset? cancelledAt)
    {
        Id = id;
        MemberId = memberId;
        MembershipPlanId = membershipPlanId;
        StartDate = startDate;
        EndDate = endDate;
        UpdatedAt = updatedAt;
        CancelledAt = cancelledAt;
    }
}
