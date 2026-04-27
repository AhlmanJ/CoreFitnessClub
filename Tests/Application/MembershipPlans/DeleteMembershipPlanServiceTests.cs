
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction;
using Application.Common.Results;
using Application.MembershipPlans.Services;
using Domain.Abstractions.Repositories.MembershipPlans;
using NSubstitute;

namespace Tests.Application.MembershipPlans;

public class DeleteMembershipPlanServiceTests
{
    private readonly IMembershipPlanRepository _membershipPlanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteMembershipPlanService _service;

    public DeleteMembershipPlanServiceTests()
    {
        _membershipPlanRepository = Substitute.For<IMembershipPlanRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _service = new DeleteMembershipPlanService(
            _membershipPlanRepository,
            _unitOfWork
        );
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
        Assert.Equal("Id cannot be empty", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenPlanDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        _membershipPlanRepository
            .GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Domain.Aggregates.MembershipPlan.MembershipPlan?)null);

        // Act
        var result = await _service.ExecuteAsync(id);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.NotFound, result.ErrorTypes);
        Assert.Equal("Could not fint the membership plan.", result.ErrorMessage);

        await _membershipPlanRepository
            .DidNotReceive()
            .RemoveAsync(Arg.Any<Domain.Aggregates.MembershipPlan.MembershipPlan>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenPlanIsDeleted()
    {
        // Arrange
        var id = Guid.NewGuid();

        var plan = Domain.Aggregates.MembershipPlan.MembershipPlan.Create(
            "Gold",
            "Desc",
            "A",
            "B",
            "C",
            199,
            30
        );

        _membershipPlanRepository
            .GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(plan);

        // Act
        var result = await _service.ExecuteAsync(id);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("The membership plan was deleted", result.Value);

        await _membershipPlanRepository
            .Received(1)
            .RemoveAsync(plan);

        await _unitOfWork
            .Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }
}
