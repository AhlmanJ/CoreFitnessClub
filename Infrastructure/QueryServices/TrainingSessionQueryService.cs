using Application.Abstraction.TrainingSessionQueryInterface;
using Application.TrainingSessions.Outputs;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.QueryServices;

public class TrainingSessionQueryService(DataContext context) : ITrainingSessionQueryService
{
    public async Task<List<TrainingSessionQueryOutput>> GetAllTrainingSessionsAsync(CancellationToken ct = default)
    {
        return await context.TrainingSessions
            .Select(x => new TrainingSessionQueryOutput
            (
                x.Id,
                x.TrainerMemberId,
                x.SessionName,
                x.TrainerMember.FirstName ?? "",
                x.TrainerMember.LastName ?? "",
                x.CreatedAt,
                x.StartDate,
                x.EndDate,
                x.Capacity,
                x.Location

            )).ToListAsync();
    }

    public async Task<TrainingSessionQueryOutput?> GetTrainingSessionByIdAsync(Guid Id, CancellationToken ct = default)
    {
        return await context.TrainingSessions
            .Where(x => x.Id == Id)
            .Select(x => new TrainingSessionQueryOutput
            (
                x.Id,
                x.TrainerMemberId,
                x.SessionName,
                x.TrainerMember.FirstName ?? "",
                x.TrainerMember.LastName ?? "",
                x.CreatedAt,
                x.StartDate,
                x.EndDate,
                x.Capacity,
                x.Location

            )).FirstOrDefaultAsync(ct);
    }
}
