using Domain.Aggregates.TraingSessions;
using Domain.Persistence.Repositories;

namespace Domain.Abstractions.Repositories.TrainingSessions
{
    public interface ITrainingSessionRepository : IRepositoryBase<TrainingSession, Guid>
    {

    }
}