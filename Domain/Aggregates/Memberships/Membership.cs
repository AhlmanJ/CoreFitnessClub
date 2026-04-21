
/*
 * I moved the entities to the Infrastructure layer to follow Clean Architecture and the Domain layer Aggregates are strict for domain logic.
 */

namespace Domain.Aggregates.Memberships;

public class Membership
{
    public Guid Id { get; private set; }

    public Guid MemberId { get; private set; }

    public Guid MembershipPlanId { get; private set; }

    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? CancelledAt { get; private set; }


    private Membership() 
    {
    
    }

    private Membership(Guid id, Guid memberId, Guid membershipPlanId, DateTimeOffset startDate, DateTimeOffset endDate)
    {
        Id = id;
        MemberId = memberId;
        MembershipPlanId = membershipPlanId;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static Membership Create(Guid memberId, Guid membershipPlanId, DateTimeOffset startDate, DateTimeOffset endDate) 
    {
        if (memberId == Guid.Empty)
            throw new ArgumentException("MemberId cannot be empty.");

        if (membershipPlanId == Guid.Empty)
            throw new ArgumentException("MembershipPlanId cannot be empty.");

        if (startDate > endDate)
        {
            throw new ArgumentException("Start date cannot be greater than end date.");
        }

        var membership = new Membership(
            Guid.NewGuid(),
            memberId,
            membershipPlanId,
            startDate,
            endDate
            );

        return membership;
    }

    public static Membership Rehydrate(Guid id, Guid memberId, Guid membershipPlanId, DateTimeOffset startDate, DateTimeOffset endDate, DateTimeOffset? updatedAt = null, DateTimeOffset? cancelledAt = null)
    {

        return new Membership(id, memberId, membershipPlanId, startDate, endDate)
        {
            UpdatedAt = updatedAt,
            CancelledAt = cancelledAt
        };
    }

    public void ChangeDates(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        if (startDate > endDate)
        {
            throw new ArgumentException("Start date cannot be greater than end date.");
        }

        StartDate = startDate;
        EndDate = endDate;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
