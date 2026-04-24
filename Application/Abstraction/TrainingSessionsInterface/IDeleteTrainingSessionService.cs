using Application.Common.Results;
using Application.TrainingSessions.Inputs;
using Application.TrainingSessions.Outputs;

namespace Application.Abstraction.TrainingSessionsInterface;

public interface IDeleteTrainingSessionService
{
    Task<Result<string?>> ExecuteAsync(Guid Id, CancellationToken ct = default);
}
