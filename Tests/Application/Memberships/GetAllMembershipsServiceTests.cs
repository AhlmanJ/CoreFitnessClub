
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction.MembershipReadInterface;
using Application.Common.Results;
using Application.Memberships.Outputs;
using Application.Memberships.Services;
using NSubstitute;

namespace Tests.Application.Memberships;

public class GetAllMembershipsServiceTests
{
    private readonly IMembershipQueryService _membershipQueryService;
    private readonly GetAllMembershipsService _service;

    public GetAllMembershipsServiceTests()
    {
        _membershipQueryService = Substitute.For<IMembershipQueryService>();

        _service = new GetAllMembershipsService(
            _membershipQueryService
        );
    }

    private List<MembershipResponseOutput> CreateOutput() =>
    [
        new MembershipResponseOutput(
            Guid.NewGuid(),
            "John",
            "Doe",
            "Gold",
            199,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddDays(30)
        )
    ];

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenMembershipsExist()
    {
        // Arrange
        var expected = CreateOutput();

        _membershipQueryService
            .GetAllMembershipsAsync(Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _service.ExecuteAsync();

        // Assert
        Assert.True(result.Success);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        _membershipQueryService
            .GetAllMembershipsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromException<List<MembershipResponseOutput>>(
                new Exception("DB error")));

        // Act
        var result = await _service.ExecuteAsync();

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal("DB error", result.ErrorMessage);
    }
}
