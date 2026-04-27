
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction;
using Application.Bookings.Services;
using Application.Common.Results;
using Domain.Abstractions.Repositories.Booking;
using Domain.Abstractions.Repositories.Memberships;
using Domain.Abstractions.Repositories.TrainingSessions;
using Domain.Aggregates.TraingSessions;
using NSubstitute;

namespace Tests.Application.Bookings;

public class DeleteBookingServiceTests
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ITrainingSessionRepository _trainingSessionRepository;
    private readonly IMembershipRepository _membershipRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteBookingService _service;

    public DeleteBookingServiceTests()
    {
        _bookingRepository = Substitute.For<IBookingRepository>();
        _trainingSessionRepository = Substitute.For<ITrainingSessionRepository>();
        _membershipRepository = Substitute.For<IMembershipRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _service = new DeleteBookingService(
            _bookingRepository,
            _trainingSessionRepository,
            _unitOfWork);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenIdIsEmpty()
    {
        // Arrange
        var id = Guid.Empty;

        // Act
        var result = await _service.ExecuteAsync(id);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.BadRequest, result.ErrorTypes);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenBookingDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        _bookingRepository
            .GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Domain.Aggregates.Bookings.Bookings?)null);

        // Act
        var result = await _service.ExecuteAsync(id);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.NotFound, result.ErrorTypes);

        await _trainingSessionRepository
            .DidNotReceive()
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenTrainingSessionDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var sessionId = Guid.NewGuid();

        var booking = Domain.Aggregates.Bookings.Bookings.Create(
            Guid.NewGuid(),
            sessionId
        );

        _bookingRepository
            .GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(booking);

        _trainingSessionRepository
            .GetByIdAsync(sessionId, Arg.Any<CancellationToken>())
            .Returns((TrainingSession?)null);

        // Act
        var result = await _service.ExecuteAsync(id);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.NotFound, result.ErrorTypes);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenBookingIsDeleted()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var booking = Domain.Aggregates.Bookings.Bookings.Rehydrate(
            bookingId,
            memberId,
            sessionId,
            Domain.Aggregates.Bookings.BookingStatus.Booked,
            DateTimeOffset.UtcNow
        );

        var trainingSession = TrainingSession.Rehydrate(
            sessionId,
            Guid.NewGuid(),
            "Test Session",
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddHours(1),
            9,
            "Gym"
        );

        _bookingRepository
            .GetByIdAsync(bookingId, Arg.Any<CancellationToken>())
            .Returns(booking);

        _trainingSessionRepository
            .GetByIdAsync(sessionId, Arg.Any<CancellationToken>())
            .Returns(trainingSession);

        _trainingSessionRepository
            .UpdateAsync(trainingSession, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _service.ExecuteAsync(bookingId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("The booking was deleted", result.Value);

        await _trainingSessionRepository
            .Received(1)
            .UpdateAsync(trainingSession, Arg.Any<CancellationToken>());

        await _bookingRepository
            .Received(1)
            .RemoveAsync(booking, Arg.Any<CancellationToken>());

        await _unitOfWork
            .Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }
}
