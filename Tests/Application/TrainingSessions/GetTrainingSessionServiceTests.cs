
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction.TrainingSessionQueryInterface;
using Application.Common.Results;
using Application.TrainingSessions.Outputs;
using Application.TrainingSessions.Services;
using NSubstitute;

namespace Tests.Application.TrainingSessions;

public class GetTrainingSessionServiceTests
{
    private readonly ITrainingSessionQueryService _trainingSessionQueryService;
    private readonly GetTrainingSessionService _service;

    // -------------------------
    // Constructor (DI setup)
    // -------------------------
    public GetTrainingSessionServiceTests()
    {
        _trainingSessionQueryService = Substitute.For<ITrainingSessionQueryService>();

        _service = new GetTrainingSessionService(
            _trainingSessionQueryService
        );
    }

    // Helper: output
    private TrainingSessionQueryOutput CreateSession()
    {
        return new TrainingSessionQueryOutput(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Yoga",
            "John",
            "Doe",
            DateTimeOffset.UtcNow,
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
        Assert.Equal("Training session ID must be provided.", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenSessionDoesNotExist()
    {
        var id = Guid.NewGuid();

        _trainingSessionQueryService
            .GetTrainingSessionByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((TrainingSessionQueryOutput?)null);

        var result = await _service.ExecuteAsync(id);

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.NotFound, result.ErrorTypes);
        Assert.Equal("The requested training session could not be found.", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenSessionExists()
    {
        var id = Guid.NewGuid();
        var session = CreateSession();

        _trainingSessionQueryService
            .GetTrainingSessionByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(session);

        var result = await _service.ExecuteAsync(id);

        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal(session.Id, result.Value!.Id);
        Assert.Equal(session.SessionName, result.Value.SessionName);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        var id = Guid.NewGuid();

        _trainingSessionQueryService
            .GetTrainingSessionByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<TrainingSessionQueryOutput?>(
                new Exception("DB error")));

        var result = await _service.ExecuteAsync(id);

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal("DB error", result.ErrorMessage);
    }
}
