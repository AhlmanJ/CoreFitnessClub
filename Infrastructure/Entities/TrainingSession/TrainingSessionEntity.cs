
// Here i got help from chatGPT on how to create a relation so that a Member can be a trainer.

using Infrastructure.Entities.Booking;
using Infrastructure.Entities.Members;

namespace Infrastructure.Entities.TrainingSession;

public class TrainingSessionEntity
{
    public Guid Id { get; internal set; }
    public Guid TrainerMemberId { get; internal set; }
    public MemberEntity TrainerMember { get; internal set; } = null!;
    public string SessionName { get; internal set; } = null!;
    public DateTimeOffset CreatedAt { get; internal set; } = DateTimeOffset.Now;
    public byte[] RowVersion { get; internal set; } = null!;
    public DateTimeOffset StartDate { get; internal set; }
    public DateTimeOffset EndDate { get; internal set; }
    public int Capacity { get; internal set; } = 0;
    public string Location { get; internal set; } = null!;


    public ICollection<BookingsEntity> Bookings { get; private set; } = new List<BookingsEntity>();

    private TrainingSessionEntity ()
    { 
    
    }

    public TrainingSessionEntity (Guid id, Guid trainerMemberId, string sessionName, DateTimeOffset createdAt, DateTimeOffset startDate, DateTimeOffset endDate, int capacity, string location)
    {
        Id = id;
        TrainerMemberId = trainerMemberId;
        SessionName = sessionName;
        CreatedAt = createdAt;
        StartDate = startDate;
        EndDate = endDate;
        Capacity = capacity;
        Location = location;
    }
}
