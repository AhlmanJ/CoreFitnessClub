
using Application.Bookings.Outputs;

namespace Application.Abstraction.BookingsQueryInterface;

public interface IBookingsQueryService
{
    Task<List<BookingsQueryOutput>> GetBookingsByMemberIdAsync(Guid memberId, CancellationToken ct = default);
}
