using Domain.Entities.Members;
using Domain.Entities.TrainingSession;

namespace Domain.Entities.Booking;

public enum BookingStatus
{
    Booked,
    Cancelled,
    Attended
};

public class BookingEntity
{
    public Guid Id { get; private set; }

    public Guid MemberId { get; private set; }
    public virtual MemberEntity Member { get; private set; } = null!;


    public Guid TrainingSessionId { get; private set; }
    public virtual TrainingSessionEntity TrainingSession { get; private set; } = null!;


    public BookingStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }


    private BookingEntity() { }

    public BookingEntity(Guid memberId, Guid trainingSessionId, BookingStatus status, DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        MemberId = memberId;
        TrainingSessionId = trainingSessionId;
        Status = BookingStatus.Booked;
        CreatedAt = createdAt;
    }

    public BookingEntity(Guid id, Guid memberId, Guid trainingSessionId, BookingStatus status, DateTimeOffset createdAt)
    {  
        Id = id;
        MemberId = memberId;
        TrainingSessionId = trainingSessionId;
        Status = status; 
        CreatedAt = createdAt;
    }

    public void UpdateBooking(BookingStatus status)
    {
        Status = status;
    }
}
