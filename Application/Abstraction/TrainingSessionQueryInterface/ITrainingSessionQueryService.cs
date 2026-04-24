using Application.Memberships.Outputs;
using Application.TrainingSessions.Outputs;

namespace Application.Abstraction.TrainingSessionQueryInterface;

public interface ITrainingSessionQueryService
{
    Task<TrainingSessionQueryOutput?> GetTrainingSessionByIdAsync(Guid Id, CancellationToken ct = default);
    Task<List<TrainingSessionQueryOutput>> GetAllTrainingSessionsAsync(CancellationToken ct = default);
}
