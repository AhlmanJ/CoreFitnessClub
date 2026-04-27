using Application.Abstraction;
using Application.Abstraction.TrainingSessionsInterface;
using Application.Common.Results;
using Application.TrainingSessions.Inputs;
using Application.TrainingSessions.Outputs;
using Domain.Abstractions.Repositories.TrainingSessions;
using Domain.Aggregates.TraingSessions;


namespace Application.TrainingSessions.Services;

public class CreateTrainingSessionService(ITrainingSessionRepository trainingSessionRepository, IUnitOfWork unitOfWork) : ICreateTrainingSessionService
{
    public async Task<Result<TrainingSessionOutput>> ExecuteAsync(CreateTrainingSessionInput input, CancellationToken ct = default)
    {
        if (input == null)
            return Result<TrainingSessionOutput>.BadRequest("The input was empty.");

        try 
        {
            var trainingSession = TrainingSession.Create
                (
                    input.TrainerMemberId,
                    input.SessionName,
                    input.StartDate,
                    input.EndDate,
                    input.Capacity,
                    input.Location
                );

            await trainingSessionRepository.AddAsync(trainingSession, ct);
            await unitOfWork.CommitAsync(ct);

            var result = new TrainingSessionOutput
                (
                    trainingSession.Id,
                    trainingSession.TrainerMemberId,
                    trainingSession.SessionName,
                    trainingSession.CreatedAt,
                    trainingSession.StartDate,
                    trainingSession.EndDate,
                    trainingSession.Capacity,
                    trainingSession.Location
                );

            return Result<TrainingSessionOutput>.Ok(result);

        }
        catch (Exception ex)
        {
            return Result<TrainingSessionOutput>.InternalServerError(ex.Message);
        }
    }
}
