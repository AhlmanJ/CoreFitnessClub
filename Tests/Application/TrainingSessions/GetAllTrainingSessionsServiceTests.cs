
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction.TrainingSessionQueryInterface;
using Application.Common.Results;
using Application.TrainingSessions.Outputs;
using Application.TrainingSessions.Services;
using NSubstitute;

namespace Tests.Application.TrainingSessions;

public class GetAllTrainingSessionsServiceTests
{
    private readonly ITrainingSessionQueryService _trainingSessionQueryService;
    private readonly GetAllTrainingSessionsService _service;

    // -------------------------
    // Constructor (DI setup)
    // -------------------------
    public GetAllTrainingSessionsServiceTests()
    {
        _trainingSessionQueryService = Substitute.For<ITrainingSessionQueryService>();

        _service = new GetAllTrainingSessionsService(
            _trainingSessionQueryService
        );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnEmptyList_WhenNullReturned()
    {
        _trainingSessionQueryService
            .GetAllTrainingSessionsAsync(Arg.Any<CancellationToken>())
            .Returns(new List<TrainingSessionQueryOutput>());

        var result = await _service.ExecuteAsync();

        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnMappedSessions_WhenDataExists()
    {
        var sessions = new List<TrainingSessionQueryOutput>
        {
            new TrainingSessionQueryOutput(
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
            )
        };

        _trainingSessionQueryService
            .GetAllTrainingSessionsAsync(Arg.Any<CancellationToken>())
            .Returns(sessions);

        var result = await _service.ExecuteAsync();

        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);
        Assert.Equal("Yoga", result.Value[0].SessionName);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        _trainingSessionQueryService
            .GetAllTrainingSessionsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromException<List<TrainingSessionQueryOutput>>(new Exception("DB error")));

        var result = await _service.ExecuteAsync();

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal("DB error", result.ErrorMessage);
    }

}
