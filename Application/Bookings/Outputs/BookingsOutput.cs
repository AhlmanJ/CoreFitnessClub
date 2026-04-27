using Domain.Aggregates.Bookings;

namespace Application.Bookings.Outputs;

public record BookingsOutput
(
    Guid Id,
    Guid MemberId,
    Guid TrainingSessionId,
    BookingStatus Status,
    DateTimeOffset CreatedAt
);
