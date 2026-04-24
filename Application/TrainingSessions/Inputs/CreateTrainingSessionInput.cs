
namespace Application.TrainingSessions.Inputs;

public record CreateTrainingSessionInput
(
    Guid TrainerMemberId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int Capacity,
    string Location
);

