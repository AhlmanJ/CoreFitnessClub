namespace Domain.Aggregates.Bookings;

public enum BookingStatus
{
    Booked,
    Cancelled,
    Attended
};

public class Bookings
{
    public Guid Id { get; private set; }

    public Guid MemberId { get; private set; }

    public Guid TrainingSessionId { get; private set; }

    public BookingStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public byte[] RowVersion { get; private set; } = null!;

    private Bookings()
    { 
    
    }

    private Bookings(Guid id, Guid memberId, Guid trainingSessionId, BookingStatus status, DateTimeOffset createdAt)
    {
        Id = id;
        MemberId = memberId;
        TrainingSessionId = trainingSessionId;
        Status = status;
        CreatedAt = createdAt;
    }

    public static Bookings Create(Guid memberId, Guid trainingSessionId)
    {
        return new Bookings
        (
            Guid.NewGuid(),
            memberId,
            trainingSessionId,
            BookingStatus.Booked,
            DateTimeOffset.UtcNow
        );
    }

    public static Bookings Rehydrate(Guid id, Guid memberId, Guid trainingSessionId, BookingStatus status, DateTimeOffset createdAt)
    {
        return new Bookings(id, memberId, trainingSessionId, status, createdAt)
        {

        };
    }
}
