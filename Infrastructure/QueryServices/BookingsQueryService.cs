using Application.Abstraction.BookingsQueryInterface;
using Application.Bookings.Outputs;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.QueryServices;

class BookingsQueryService(DataContext context) : IBookingsQueryService
{
    public async Task<List<BookingsQueryOutput>> GetBookingsByMemberIdAsync(Guid memberId, CancellationToken ct = default)
    {
        return await context.Bookings
            .Where(b => b.MemberId == memberId)
            .Select(b => new BookingsQueryOutput
            (
                b.Id,
                b.TrainingSession.SessionName,
                b.TrainingSession.TrainerMember.FirstName!,
                b.TrainingSession.TrainerMember.LastName!,
                b.TrainingSession.StartDate,
                b.TrainingSession.EndDate
            )).ToListAsync(ct);
    }
}
