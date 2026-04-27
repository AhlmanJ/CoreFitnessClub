
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Common.Results;
using Application.MembershipPlans.Services;
using Domain.Abstractions.Repositories.MembershipPlans;
using NSubstitute;

namespace Tests.Application.MembershipPlans;

public class GetAllMembershipPlansServiceTests
{
    private readonly IMembershipPlanRepository _membershipPlanRepository;
    private readonly GetAllMembershipPlansService _service;

    public GetAllMembershipPlansServiceTests()
    {
        _membershipPlanRepository = Substitute.For<IMembershipPlanRepository>();

        _service = new GetAllMembershipPlansService(_membershipPlanRepository);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnEmptyList_WhenRepositoryReturnsNull()
    {
        // Arrange
        _membershipPlanRepository
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns((List<Domain.Aggregates.MembershipPlan.MembershipPlan>?)null!);

        // Act
        var result = await _service.ExecuteAsync();

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnMappedMembershipPlans_WhenDataExists()
    {
        // Arrange
        var plans = new List<Domain.Aggregates.MembershipPlan.MembershipPlan>
        {
            Domain.Aggregates.MembershipPlan.MembershipPlan.Create(
                "Gold",
                "Desc",
                "A",
                "B",
                "C",
                199,
                30
            ),
            Domain.Aggregates.MembershipPlan.MembershipPlan.Create(
                "Silver",
                "Desc",
                "A",
                "B",
                "C",
                99,
                15
            )
        };

        _membershipPlanRepository
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(plans);

        // Act
        var result = await _service.ExecuteAsync();

        // Assert
        Assert.True(result.Success);
        Assert.Equal(2, result.Value!.Count);

        Assert.Equal("Gold", result.Value[0].Name);
        Assert.Equal("Silver", result.Value[1].Name);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
         _membershipPlanRepository
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromException<IReadOnlyList<Domain.Aggregates.MembershipPlan.MembershipPlan>>(
            new Exception("DB error")
            ));

        // Act 
        var result = await _service.ExecuteAsync();

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal("DB error", result.ErrorMessage);
    }
}
