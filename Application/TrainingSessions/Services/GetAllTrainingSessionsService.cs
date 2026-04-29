using Application.Abstraction.TrainingSessionQueryInterface;
using Application.Abstraction.TrainingSessionsInterface;
using Application.Common.Results;
using Application.TrainingSessions.Outputs;

namespace Application.TrainingSessions.Services;

public class GetAllTrainingSessionsService(ITrainingSessionQueryService trainingSessionQueryService) : IGetAllTrainingSessionsService
{
    public async Task<Result<List<TrainingSessionQueryOutput>>> ExecuteAsync(CancellationToken ct = default)
    {

        try
        {
            var trainingSessions = await trainingSessionQueryService.GetAllTrainingSessionsAsync(ct);
            if (trainingSessions == null)
                return Result<List<TrainingSessionQueryOutput>>.Ok(new List<TrainingSessionQueryOutput>());

            var output = trainingSessions.Select(sessions => new TrainingSessionQueryOutput(
                    
                    sessions.Id,
                    sessions.TrainerMemberId,
                    sessions.SessionName,
                    sessions.TrainerFirstName,
                    sessions.TrainerLastName,
                    sessions.CreatedAt,
                    sessions.StartDate,
                    sessions.EndDate,
                    sessions.Capacity,
                    sessions.Location

                )).ToList();

            return Result<List<TrainingSessionQueryOutput>>.Ok(output);

        }
        catch (Exception ex)
        {
            return Result<List<TrainingSessionQueryOutput>>.InternalServerError(ex.Message);
        }
    }
}
