namespace Application.TrainingSessions.Outputs;

public record TrainingSessionQueryOutput
(
    Guid Id,
    Guid TrainerMemberId,
    string TrainerFirstName,
    string TrainerLastName,
    DateTimeOffset CreatedAt,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int Capacity,
    string Location
);
