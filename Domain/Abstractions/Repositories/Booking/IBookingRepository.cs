using Domain.Aggregates.Bookings;
using Domain.Persistence.Repositories;

namespace Domain.Abstractions.Repositories.Booking;

public interface IBookingRepository : IRepositoryBase<Bookings, Guid>
{
    Task<bool> ExistsSync(Guid memberId, Guid trainingSession, CancellationToken ct = default); 
}
