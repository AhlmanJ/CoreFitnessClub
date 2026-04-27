using Domain.Aggregates.Bookings;
using Infrastructure.Entities.Members;
using Infrastructure.Entities.TrainingSession;

namespace Infrastructure.Entities.Booking;

public class BookingsEntity
{
    public Guid Id { get; private set; }

    public Guid MemberId { get; private set; }
    public virtual MemberEntity Member { get; private set; } = null!;


    public Guid TrainingSessionId { get; private set; }
    public virtual TrainingSessionEntity TrainingSession { get; private set; } = null!;


    public BookingStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public byte[] RowVersion {  get; private set; } = null!;

    private BookingsEntity() { }

    public BookingsEntity(Guid id, Guid memberId, Guid trainingSessionId, BookingStatus status, DateTimeOffset createdAt)
    {  
        Id = id;
        MemberId = memberId;
        TrainingSessionId = trainingSessionId;
        Status = status; 
        CreatedAt = createdAt;
    }
}
