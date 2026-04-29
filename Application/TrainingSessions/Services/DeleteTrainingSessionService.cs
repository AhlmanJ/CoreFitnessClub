using Application.Abstraction;
using Application.Abstraction.TrainingSessionsInterface;
using Application.Common.Results;
using Domain.Abstractions.Repositories.TrainingSessions;

namespace Application.TrainingSessions.Services;

public class DeleteTrainingSessionService(ITrainingSessionRepository trainingSessionRepository, IUnitOfWork unitOfWork) : IDeleteTrainingSessionService
{
    public async Task<Result<string?>> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
            return Result<string?>.BadRequest("Id cannot be empty");

        try 
        { 
            var sessionToDelete = await trainingSessionRepository.GetByIdAsync(id, ct);
            if(sessionToDelete == null)
                return Result<string?>.NotFound("Could not find the trainingsession.");

            await trainingSessionRepository.RemoveAsync(sessionToDelete);
            await unitOfWork.CommitAsync(ct);

            return Result<string?>.Ok("The training session was deleted.");
        }
        catch (Exception ex)
        {
            return Result<string?>.InternalServerError(ex.Message);
        }
    }
}
