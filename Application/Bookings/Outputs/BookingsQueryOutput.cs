namespace Application.Bookings.Outputs;

public record BookingsQueryOutput
(   
    Guid Id,
    string SessionName,
    string TrainerFirstName,
    string TrainerLastName,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate
);
