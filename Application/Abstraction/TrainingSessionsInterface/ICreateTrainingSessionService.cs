using Application.Common.Results;
using Application.TrainingSessions.Inputs;
using Application.TrainingSessions.Outputs;

namespace Application.Abstraction.TrainingSessionsInterface;

    public interface ICreateTrainingSessionService
    {
        Task<Result<TrainingSessionOutput>> ExecuteAsync(CreateTrainingSessionInput input, CancellationToken ct = default);
    }
