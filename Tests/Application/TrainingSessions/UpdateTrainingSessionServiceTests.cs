
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction;
using Application.Common.Results;
using Application.TrainingSessions.Inputs;
using Application.TrainingSessions.Services;
using Domain.Abstractions.Repositories.TrainingSessions;
using Domain.Aggregates.TraingSessions;
using NSubstitute;

namespace Tests.Application.TrainingSessions;

public class UpdateTrainingSessionServiceTests
{
    private readonly ITrainingSessionRepository _trainingSessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateTrainingSessionService _service;

    // -------------------------
    // Constructor (DI setup)
    // -------------------------
    public UpdateTrainingSessionServiceTests()
    {
        _trainingSessionRepository = Substitute.For<ITrainingSessionRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _service = new UpdateTrainingSessionService(
            _trainingSessionRepository,
            _unitOfWork
        );
    }

    // Helper: input
    private UpdateTrainingSessionInput CreateInput()
    {
        return new UpdateTrainingSessionInput(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Updated Session",
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddHours(1),
            15,
            "New Gym"
        );
    }

    // Helper: domain object
    private TrainingSession CreateSession()
    {
        return TrainingSession.Create(
            Guid.NewGuid(),
            "Old Session",
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddHours(1),
            10,
            "Old Gym"
        );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenInputIsNull()
    {
        var result = await _service.ExecuteAsync(null!);

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.BadRequest, result.ErrorTypes);
        Assert.Equal("Input must be provided", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenSessionDoesNotExist()
    {
        var input = CreateInput();

        _trainingSessionRepository
            .GetByIdAsync(input.Id, Arg.Any<CancellationToken>())
            .Returns((TrainingSession?)null);

        var result = await _service.ExecuteAsync(input);

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.NotFound, result.ErrorTypes);
        Assert.Equal("Could not find the requested training session", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenUpdateFails()
    {
        var input = CreateInput();
        var session = CreateSession();

        _trainingSessionRepository
            .GetByIdAsync(input.Id, Arg.Any<CancellationToken>())
            .Returns(session);

        _trainingSessionRepository
            .UpdateAsync(session, Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await _service.ExecuteAsync(input);

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal($"Training session with Id {input.Id} was not updated.", result.ErrorMessage);

        await _unitOfWork
            .DidNotReceive()
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenUpdateSucceeds()
    {
        var input = CreateInput();
        var session = CreateSession();

        _trainingSessionRepository
            .GetByIdAsync(input.Id, Arg.Any<CancellationToken>())
            .Returns(session);

        _trainingSessionRepository
            .UpdateAsync(session, Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await _service.ExecuteAsync(input);

        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal(session.Id, result.Value!.Id);

        await _unitOfWork
            .Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        var input = CreateInput();

        _trainingSessionRepository
            .GetByIdAsync(input.Id, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<TrainingSession?>(
                new Exception("DB error")));

        var result = await _service.ExecuteAsync(input);

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal("DB error", result.ErrorMessage);
    }
}
