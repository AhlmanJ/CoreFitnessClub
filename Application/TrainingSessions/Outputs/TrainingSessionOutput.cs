
namespace Application.TrainingSessions.Outputs;

public record TrainingSessionOutput
(
    Guid Id,
    Guid TrainerMemberId,
    string SessionName,
    DateTimeOffset CreatedAt,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int Capacity,
    string Location
);