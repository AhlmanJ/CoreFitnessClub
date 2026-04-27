
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction;
using Application.Common.Results;
using Application.TrainingSessions.Services;
using Domain.Abstractions.Repositories.TrainingSessions;
using Domain.Aggregates.TraingSessions;
using NSubstitute;

namespace Tests.Application.TrainingSessions;

public class DeleteTrainingSessionServiceTests
{
    private readonly ITrainingSessionRepository _trainingSessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteTrainingSessionService _service;
    public DeleteTrainingSessionServiceTests()
    {
        _trainingSessionRepository = Substitute.For<ITrainingSessionRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _service = new DeleteTrainingSessionService(
            _trainingSessionRepository,
            _unitOfWork
        );
    }

    // Helper: create domain object
    private TrainingSession CreateSession()
    {
        return TrainingSession.Create(
            Guid.NewGuid(),
            "Yoga",
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddHours(1),
            10,
            "Gym"
        );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenIdIsEmpty()
    {
        var result = await _service.ExecuteAsync(Guid.Empty);

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.BadRequest, result.ErrorTypes);
        Assert.Equal("Id cannot be empty", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenSessionDoesNotExist()
    {
        var id = Guid.NewGuid();

        _trainingSessionRepository
            .GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((TrainingSession?)null);

        var result = await _service.ExecuteAsync(id);

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.NotFound, result.ErrorTypes);
        Assert.Equal("Could not find the trainingsession.", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenSessionIsDeleted()
    {
        var id = Guid.NewGuid();
        var session = CreateSession();

        _trainingSessionRepository
            .GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(session);

        var result = await _service.ExecuteAsync(id);

        Assert.True(result.Success);
        Assert.Equal("The training session was deleted.", result.Value);

        await _trainingSessionRepository
            .Received(1)
            .RemoveAsync(session, Arg.Any<CancellationToken>());

        await _unitOfWork
            .Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        var id = Guid.NewGuid();

        _trainingSessionRepository
            .GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns<Task<TrainingSession?>>(_ => throw new Exception("DB error"));

        var result = await _service.ExecuteAsync(id);

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal("DB error", result.ErrorMessage);
    }
}
