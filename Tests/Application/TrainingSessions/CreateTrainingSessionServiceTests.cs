
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction;
using Application.Common.Results;
using Application.TrainingSessions.Inputs;
using Application.TrainingSessions.Services;
using Domain.Abstractions.Repositories.TrainingSessions;
using NSubstitute;

namespace Tests.Application.TrainingSessions;

public class CreateTrainingSessionServiceTests
{
    private readonly ITrainingSessionRepository _trainingSessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateTrainingSessionService _service;

    // ✅ CONSTRUCTOR (Dependency Injection for tests)
    public CreateTrainingSessionServiceTests()
    {
        _trainingSessionRepository = Substitute.For<ITrainingSessionRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _service = new CreateTrainingSessionService(
            _trainingSessionRepository,
            _unitOfWork
        );
    }

    // Helper: input
    private CreateTrainingSessionInput CreateInput()
    {
        return new CreateTrainingSessionInput(
            Guid.NewGuid(),
            "Morning Yoga",
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddHours(1),
            10,
            "Gym Hall A"
        );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenInputIsNull()
    {
        var result = await _service.ExecuteAsync(null!);

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.BadRequest, result.ErrorTypes);
        Assert.Equal("The input was empty.", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenTrainingSessionIsCreated()
    {
        var input = CreateInput();

        var result = await _service.ExecuteAsync(input);

        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal(input.SessionName, result.Value!.SessionName);

        await _trainingSessionRepository
            .Received(1)
            .AddAsync(Arg.Any<Domain.Aggregates.TraingSessions.TrainingSession>(), Arg.Any<CancellationToken>());

        await _unitOfWork
            .Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        var input = CreateInput();

        _trainingSessionRepository
            .AddAsync(Arg.Any<Domain.Aggregates.TraingSessions.TrainingSession>(), Arg.Any<CancellationToken>())
            .Returns(_ => throw new Exception("DB error"));

        var result = await _service.ExecuteAsync(input);

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal("DB error", result.ErrorMessage);
    }
}
