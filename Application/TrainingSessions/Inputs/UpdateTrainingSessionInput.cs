namespace Application.TrainingSessions.Inputs;

public record UpdateTrainingSessionInput
(
    Guid Id,
    Guid TrainerMemberId,
    string SessionName,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int Capacity,
    string Location
);
