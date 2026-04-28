
// See comment on row 16!

using Domain.Abstractions.Repositories.Booking;
using Domain.Aggregates.Bookings;
using Infrastructure.Entities.Booking;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.BookingsRepo;

public class BookingsRepository(DataContext context) : RepositoryBase<Bookings, Guid, BookingsEntity, DataContext>(context), IBookingRepository
{
    public async Task<bool> ExistsSync(Guid memberId, Guid trainingSession, CancellationToken ct = default)
    {
        // Code from AI that helped me find a way to check if a member already has a booking for a specific training session.

        var bookedMember = await context.Bookings
                        .AnyAsync(b => b.MemberId == memberId 
                        && b.TrainingSessionId == trainingSession 
                        && b.Status == BookingStatus.Booked
                        , ct);

        return bookedMember;
    }

    protected override Guid GetId(Bookings model)
    {
        return model.Id;
    }

    protected override Bookings ToDomainModel(BookingsEntity entity)
    {
        var model = Bookings.Rehydrate
            (
                entity.Id,
                entity.MemberId,
                entity.TrainingSessionId,
                entity.Status,
                entity.CreatedAt
            );

        return model;
    }

    protected override BookingsEntity ToEntity(Bookings model)
    {
        var entity = new BookingsEntity
            (
                model.Id,
                model.MemberId,
                model.TrainingSessionId,
                model.Status,
                model.CreatedAt
            );

        return entity;
    }

    // Not implemented. 
    protected override void UpdateEntity(BookingsEntity entity, Bookings model)
    {
        throw new NotImplementedException();
    }
}
