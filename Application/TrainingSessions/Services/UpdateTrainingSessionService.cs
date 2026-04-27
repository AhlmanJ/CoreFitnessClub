using Application.Abstraction;
using Application.Abstraction.TrainingSessionsInterface;
using Application.Common.Results;
using Application.Members.Outputs;
using Application.TrainingSessions.Inputs;
using Application.TrainingSessions.Outputs;
using Domain.Abstractions.Repositories.TrainingSessions;
using Domain.Aggregates.TraingSessions;

namespace Application.TrainingSessions.Services;

public class UpdateTrainingSessionService(ITrainingSessionRepository trainingSessionRepository, IUnitOfWork unitOfWork) : IUpdateTrainingSessionService
{
    public async Task<Result<TrainingSessionOutput>> ExecuteAsync(UpdateTrainingSessionInput input, CancellationToken ct = default)
    {
        if (input == null)
            return Result<TrainingSessionOutput>.BadRequest("Input must be provided");

        try
        {
            var trainingSession = await trainingSessionRepository.GetByIdAsync(input.Id, ct);
            if (trainingSession == null)
                return Result<TrainingSessionOutput>.NotFound("Could not find the requested training session");

            trainingSession.UpdateTrainingSession(input.TrainerMemberId,input.SessionName, input.StartDate, input.EndDate, input.Capacity, input.Location);

            var result = await trainingSessionRepository.UpdateAsync(trainingSession, ct);

            if (!result)
                return Result<TrainingSessionOutput>.InternalServerError($"Training session with Id {input.Id} was not updated.");

            /*
             * Discovered a bug when i was testing. I checked the result of "result" after unitOfWork.CommitAsync(ct). So even if the result failed,
             * the data was saved to the database. This has now been fixed and the test succeeds.
             * 
             * Tests created by AI / chatGPT.
             * 
             */

            await unitOfWork.CommitAsync(ct);

            return Result<TrainingSessionOutput>.Ok
                (new TrainingSessionOutput
                    (  
                        trainingSession.Id,
                        trainingSession.TrainerMemberId,
                        trainingSession.SessionName,
                        trainingSession.CreatedAt,
                        trainingSession.StartDate,
                        trainingSession.EndDate,
                        trainingSession.Capacity,
                        trainingSession.Location
                    )); 

        }
        catch (Exception ex)
        {
            return Result<TrainingSessionOutput>.InternalServerError(ex.Message);
        }
    }
}
