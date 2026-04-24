using Application.Common.Results;
using Application.MembershipPlans.Outputs;
using Application.TrainingSessions.Outputs;

namespace Application.Abstraction.TrainingSessionsInterface;

public interface IGetAllTrainingSessionsService
{
    Task<Result<List<TrainingSessionQueryOutput>>> ExecuteAsync(CancellationToken ct = default);
}
