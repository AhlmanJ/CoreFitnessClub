
namespace Application.TrainingSessions.Inputs;

public record CreateTrainingSessionInput
(
    Guid TrainerMemberId,
    string SessionName,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int Capacity,
    string Location
);