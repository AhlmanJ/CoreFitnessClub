
// Here i got help from chatGPT on how to create a relationship so that a Member can be a trainer.

using Infrastructure.Entities.Booking;
using Infrastructure.Entities.Members;

namespace Infrastructure.Entities.TrainingSession;

public class TrainingSessionEntity
{
    public Guid Id { get; private set; }

    public Guid TrainerMemberId { get; private set; }
    public MemberEntity TrainerMember { get; private set; } = null!;

    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;
    public byte[] RowVersion { get; private set; } = null!;

    public ICollection<BookingEntity> Bookings { get; private set; } = new List<BookingEntity>();
}
