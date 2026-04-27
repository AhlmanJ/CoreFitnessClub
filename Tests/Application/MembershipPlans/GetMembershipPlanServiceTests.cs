
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Common.Results;
using Application.MembershipPlans.Services;
using Domain.Abstractions.Repositories.MembershipPlans;
using Domain.Aggregates.MembershipPlan;
using NSubstitute;

namespace Tests.Application.MembershipPlans;

public class GetMembershipPlanServiceTests
{
    private readonly IMembershipPlanRepository _membershipPlanRepository;
    private readonly GetMembershipPlanService _service;

    public GetMembershipPlanServiceTests()
    {
        _membershipPlanRepository = Substitute.For<IMembershipPlanRepository>();
        _service = new GetMembershipPlanService(_membershipPlanRepository);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenIdIsEmpty()
    {
        // Act
        var result = await _service.ExecuteAsync(Guid.Empty);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.BadRequest, result.ErrorTypes);
        Assert.Equal("Id cannot be empty.", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenMembershipPlanDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        _membershipPlanRepository
            .GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((MembershipPlan?)null);

        // Act
        var result = await _service.ExecuteAsync(id);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.NotFound, result.ErrorTypes);
        Assert.Equal("Could not find the membership plan.", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenMembershipPlanExists()
    {
        // Arrange
        var id = Guid.NewGuid();

        var plan = MembershipPlan.Create(
            "Gold",
            "Best plan",
            "A",
            "B",
            "C",
            199,
            30
        );

        typeof(MembershipPlan)
            .GetProperty("Id")!
            .SetValue(plan, id);

        _membershipPlanRepository
            .GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(plan);

        // Act
        var result = await _service.ExecuteAsync(id);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal(id, result.Value.Id);
        Assert.Equal("Gold", result.Value.Name);
    }
}
