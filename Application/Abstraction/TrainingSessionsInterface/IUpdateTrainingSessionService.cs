using Application.Common.Results;
using Application.TrainingSessions.Inputs;
using Application.TrainingSessions.Outputs;

namespace Application.Abstraction.TrainingSessionsInterface;

public interface IUpdateTrainingSessionService
{
    Task<Result<TrainingSessionOutput>> ExecuteAsync(UpdateTrainingSessionInput input, CancellationToken ct = default);
}
