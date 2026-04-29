using Domain.Abstractions.Repositories.TrainingSessions;
using Domain.Aggregates.TraingSessions;
using Infrastructure.Entities.TrainingSession;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Repositories.TrainingSessionRepo;

public class TrainingSessionRepository(DataContext context) : RepositoryBase<TrainingSession, Guid, TrainingSessionEntity, DataContext>(context), ITrainingSessionRepository
{
    protected override void UpdateEntity(TrainingSessionEntity entity, TrainingSession model)
    {
        entity.TrainerMemberId = model.TrainerMemberId;
        entity.SessionName = model.SessionName;
        entity.StartDate = model.StartDate;
        entity.EndDate = model.EndDate;
        entity.Capacity = model.Capacity;
        entity.Location = model.Location;

    }

    protected override Guid GetId(TrainingSession model)
    {
        return model.Id;
    }

    protected override TrainingSession ToDomainModel(TrainingSessionEntity entity)
    {
        var model = TrainingSession.Rehydrate
            (
                entity.Id,
                entity.TrainerMemberId,
                entity.SessionName,
                entity.CreatedAt,
                entity.StartDate,
                entity.EndDate,
                entity.Capacity,
                entity.Location
            );

        return model;
    }

    protected override TrainingSessionEntity ToEntity(TrainingSession model)
    {
        var entity = new TrainingSessionEntity
            (
                model.Id,
                model.TrainerMemberId,
                model.SessionName,
                model.CreatedAt,
                model.StartDate,
                model.EndDate,
                model.Capacity,
                model.Location
            );

        return entity;
    }
}

