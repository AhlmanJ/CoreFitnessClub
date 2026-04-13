
// Here i got help from chatGPT on how to create a relationship so that a Member can be a trainer.

using Domain.Entities.Booking;
using Domain.Entities.Members;

namespace Domain.Entities.TrainingSession;

public class TrainingSessionEntity
{
    public Guid Id { get; private set; }

    public Guid TrainerMemberId { get; private set; }
    public MemberEntity TrainerMember { get; private set; } = null!;

    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;


    public ICollection<BookingEntity> Bookings { get; private set; } = new List<BookingEntity>();
}
