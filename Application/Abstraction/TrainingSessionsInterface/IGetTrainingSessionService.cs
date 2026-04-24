using Application.Common.Results;
using Application.TrainingSessions.Outputs;

namespace Application.Abstraction.TrainingSessionsInterface;

public interface IGetTrainingSessionService
{
    Task<Result<TrainingSessionQueryOutput?>> ExecuteAsync(Guid id, CancellationToken ct = default);
}
