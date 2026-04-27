
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction;
using Application.Common.Results;
using Application.MembershipPlans.Inputs;
using Application.MembershipPlans.Services;
using Domain.Abstractions.Repositories.MembershipPlans;
using Domain.Aggregates.MembershipPlan;
using NSubstitute;

namespace Tests.Application.MembershipPlans;

public class UpdateMembershipPlanServiceTests
{
    private readonly IMembershipPlanRepository _membershipPlanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateMembershipPlanService _service;

    public UpdateMembershipPlanServiceTests()
    {
        _membershipPlanRepository = Substitute.For<IMembershipPlanRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _service = new UpdateMembershipPlanService(
            _membershipPlanRepository,
            _unitOfWork
        );
    }

    private UpdateMembershipPlanInput CreateInput()
    {
        return new UpdateMembershipPlanInput(
            Guid.NewGuid(),
            "Gold Updated",
            "Updated desc",
            "A",
            "B",
            "C",
            199,
            60
        );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenInputIsNull()
    {
        // Act
        var result = await _service.ExecuteAsync(null!);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenPlanDoesNotExist()
    {
        // Arrange
        var input = new UpdateMembershipPlanInput(
            Guid.NewGuid(),
            "Name",
            "Desc",
            "A",
            "B",
            "C",
            100,
            30
        );

        _membershipPlanRepository
            .GetByIdAsync(input.Id, Arg.Any<CancellationToken>())
            .Returns((MembershipPlan?)null);

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.NotFound, result.ErrorTypes);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenUpdateFails()
    {
        // Arrange
        var input = CreateInput();

        var plan = MembershipPlan.Create(
            "Gold",
            "Desc",
            "A",
            "B",
            "C",
            100,
            30
        );

        _membershipPlanRepository
            .GetByIdAsync(input.Id, Arg.Any<CancellationToken>())
            .Returns(plan);

        _membershipPlanRepository
            .UpdateAsync(plan, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false));

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);

        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenUpdateSucceeds()
    {
        // Arrange
        var input = CreateInput();

        var plan = MembershipPlan.Create(
            "Gold",
            "Desc",
            "A",
            "B",
            "C",
            100,
            30
        );

        _membershipPlanRepository
            .GetByIdAsync(input.Id, Arg.Any<CancellationToken>())
            .Returns(plan);

        _membershipPlanRepository
            .UpdateAsync(plan, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(plan.Id, result.Value!.Id);
        Assert.Equal(input.Name, result.Value.Name);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

}
