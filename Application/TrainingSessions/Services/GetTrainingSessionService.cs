using Application.Abstraction.TrainingSessionQueryInterface;
using Application.Abstraction.TrainingSessionsInterface;
using Application.Common.Results;
using Application.TrainingSessions.Outputs;

namespace Application.TrainingSessions.Services;

public class GetTrainingSessionService(ITrainingSessionQueryService trainingSessionQueryService) : IGetTrainingSessionService
{
    public async Task<Result<TrainingSessionQueryOutput?>> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
            return Result<TrainingSessionQueryOutput?>.BadRequest("Training session ID must be provided.");

        try
        {
            var result = await trainingSessionQueryService.GetTrainingSessionByIdAsync(id, ct);
            if (result == null)
                return Result<TrainingSessionQueryOutput?>.NotFound("The requested training session could not be found.");

            return Result<TrainingSessionQueryOutput?>.Ok(result);
        }
        catch (Exception ex)
        {
            return Result<TrainingSessionQueryOutput?>.InternalServerError(ex.Message);
        }
    }
}
