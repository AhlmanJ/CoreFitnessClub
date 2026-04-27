

// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction;
using Application.Bookings.Inputs;
using Application.Bookings.Services;
using Application.Common.Results;
using Domain.Abstractions.Repositories.Booking;
using Domain.Abstractions.Repositories.Memberships;
using Domain.Abstractions.Repositories.TrainingSessions;
using Domain.Aggregates.TraingSessions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Tests.Application.Bookings;

public class CreateBookingServiceTests
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ITrainingSessionRepository _trainingSessionRepository;
    private readonly IMembershipRepository _membershipRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateBookingService _service;

    public CreateBookingServiceTests()
    {
        _bookingRepository = Substitute.For<IBookingRepository>();
        _trainingSessionRepository = Substitute.For<ITrainingSessionRepository>();
        _membershipRepository = Substitute.For<IMembershipRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _service = new CreateBookingService(
            _bookingRepository,
            _trainingSessionRepository,
            _membershipRepository,
            _unitOfWork);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowArgumentException_WhenIdIsEmpty()
    {
        // Arrange
        var input = new CreateBookingInput(
            Guid.Empty,
            Guid.NewGuid()
        );

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ExecuteAsync(input));

        Assert.Equal("An input must be provided.", ex.Message);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowArgumentException_WhenTrainerMemberIdIsEmpty()
    {
        // Arrange
        var input = new CreateBookingInput(
            Guid.NewGuid(),
            Guid.Empty
        );

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ExecuteAsync(input));

        Assert.Equal("An input must be provided.", ex.Message);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenTrainingSessionDoesNotExist()
    {
        // Arrange
        var input = new CreateBookingInput(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        _trainingSessionRepository
            .GetByIdAsync(input.Id, Arg.Any<CancellationToken>())
            .Returns((TrainingSession?)null);

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.BadRequest, result.ErrorTypes);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenBookingIsCreated()
    {
        // Arrange
        var trainingSessionId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var input = new CreateBookingInput(trainingSessionId, memberId);

        var trainingSession = TrainingSession.Rehydrate( // Simulate database entity.
            trainingSessionId,
            Guid.NewGuid(),
            "Test Session",
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddHours(1),
            DateTimeOffset.UtcNow.AddHours(2),
            10,
            "Gym"
        );

        _trainingSessionRepository
            .GetByIdAsync(trainingSessionId, Arg.Any<CancellationToken>())
            .Returns(trainingSession);

        _trainingSessionRepository
            .UpdateAsync(trainingSession, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal(memberId, result.Value.MemberId);
        Assert.Equal(trainingSessionId, result.Value.TrainingSessionId);

        await _trainingSessionRepository.Received(1)
            .UpdateAsync(trainingSession, Arg.Any<CancellationToken>());

        await _bookingRepository.Received(1)
            .AddAsync(Arg.Any<Domain.Aggregates.Bookings.Bookings>(), Arg.Any<CancellationToken>());

        await _unitOfWork.Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var input = new CreateBookingInput(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        _trainingSessionRepository
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Throws(new Exception("unexpected error"));

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal("unexpected error", result.ErrorMessage);
    }
}
