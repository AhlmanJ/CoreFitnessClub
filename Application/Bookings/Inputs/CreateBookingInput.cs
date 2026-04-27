namespace Application.Bookings.Inputs;

public record CreateBookingInput
(
    Guid Id,
    Guid TrainerMemberId
);
