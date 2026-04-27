
// ---------> Help by chatGPT --- see row 45.

using Application.Abstraction;
using Application.Abstraction.BookingsInterface;
using Application.Bookings.Inputs;
using Application.Bookings.Outputs;
using Application.Common.Results;
using Domain.Abstractions.Repositories.Booking;
using Domain.Abstractions.Repositories.Memberships;
using Domain.Abstractions.Repositories.TrainingSessions;

namespace Application.Bookings.Services;

public class CreateBookingService(IBookingRepository bookingsRepository,ITrainingSessionRepository trainingSessionRepository, IMembershipRepository membershipRepository, IUnitOfWork unitOfWork) : ICreateBookingService
{
    public async Task<Result<BookingsOutput>> ExecuteAsync(CreateBookingInput input, CancellationToken ct = default)
    {
        if (input.Id == Guid.Empty)
            throw new ArgumentException("An input must be provided.");

        if (input.TrainerMemberId == Guid.Empty)
            throw new ArgumentException("An input must be provided.");

        try
        {
            var trainingSession = await trainingSessionRepository.GetByIdAsync(input.Id, ct);
            if (trainingSession is null)
                return Result<BookingsOutput>.BadRequest($"Could not find a training session with ID: {input.Id}");

            var memberId = input.TrainerMemberId;

            var newBooking = Domain.Aggregates.Bookings.Bookings.Create
                (
                    memberId,
                    trainingSession.Id
                );

            /*
             * Here i had a rule to decrement the Capacity variable by one (-1) when a booking is made. However, since the domain entity should validate and contain
             * the rules that should always be "true", it was more correct to put this rule in the Domain-aggregate. I didn't know how to create a method for this,
             * so i turned to chatGPT for tips on how i could set up a method for this.
             */

            trainingSession.DecreaseCapacity();

            await trainingSessionRepository.UpdateAsync(trainingSession, ct);
            await bookingsRepository.AddAsync(newBooking, ct);
            await unitOfWork.CommitAsync(ct);

            var result = new BookingsOutput
                (
                    newBooking.Id,
                    newBooking.MemberId,
                    newBooking.TrainingSessionId,
                    newBooking.Status,
                    newBooking.CreatedAt
                );

            return Result<BookingsOutput>.Ok(result);
        }
        catch (Exception ex)
        {
            return Result<BookingsOutput>.InternalServerError(ex.Message);
        }
    }
}
