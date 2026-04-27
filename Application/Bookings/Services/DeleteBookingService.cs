using Application.Abstraction;
using Application.Abstraction.BookingsInterface;
using Application.Common.Results;
using Domain.Abstractions.Repositories.Booking;
using Domain.Abstractions.Repositories.TrainingSessions;
using Domain.Aggregates.TraingSessions;

namespace Application.Bookings.Services;

public class DeleteBookingService(IBookingRepository bookingRepository, ITrainingSessionRepository trainingSessionRepository, IUnitOfWork unitOfWork) : IDeleteBookingService
{
    public async Task<Result<string?>> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
            return Result<string?>.BadRequest("Id cannot be empty");

        try
        {
            var bookingToDelete = await bookingRepository.GetByIdAsync(id, ct);
            if (bookingToDelete == null)
                return Result<string?>.NotFound("Could not find the booking.");

            var trainingSession = await trainingSessionRepository.GetByIdAsync(bookingToDelete.TrainingSessionId, ct);
            if(trainingSession == null)
                return Result<string?>.NotFound("Could not find the training session.");

            trainingSession.IncreaseCapacity();

            await trainingSessionRepository.UpdateAsync(trainingSession, ct);
            await bookingRepository.RemoveAsync(bookingToDelete, ct);
            await unitOfWork.CommitAsync(ct);

            return Result<string?>.Ok("The booking was deleted");
        }
        catch (Exception ex)
        {
            return Result<string?>.InternalServerError(ex.Message);
        }
    }
}

