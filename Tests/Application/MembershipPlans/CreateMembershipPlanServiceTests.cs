
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction;
using Application.Common.Results;
using Application.MembershipPlans.Inputs;
using Application.MembershipPlans.Services;
using Domain.Abstractions.Repositories.MembershipPlans;
using NSubstitute;

namespace Tests.Application.MembershipPlans;

public class CreateMembershipPlanServiceTests
{
    private readonly IMembershipPlanRepository _membershipPlanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateMembershipPlanService _service;

    public CreateMembershipPlanServiceTests()
    {
        _membershipPlanRepository = Substitute.For<IMembershipPlanRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _service = new CreateMembershipPlanService(
            _membershipPlanRepository,
            _unitOfWork
        );
    }

    private CreateMembershipPlanInput CreateValidInput()
    {
        return new CreateMembershipPlanInput(
            "Gold Plan",
            "Best plan for members",
            "Access gym",
            "Free PT",
            "Sauna included",
            199m,
            30
        );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenInputIsNull()
    {
        // Act
        var result = await _service.ExecuteAsync(null!);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.BadRequest, result.ErrorTypes);
        Assert.Equal("The input was empty.", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenTwoPlansAlreadyExist()
    {
        // Arrange
        var input = CreateValidInput();

        var existingPlans = new List<Domain.Aggregates.MembershipPlan.MembershipPlan>
    {
        Domain.Aggregates.MembershipPlan.MembershipPlan.Create("A","A","1","2","3",100,30),
        Domain.Aggregates.MembershipPlan.MembershipPlan.Create("B","B","1","2","3",150,30)
    };

        _membershipPlanRepository
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(existingPlans);

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.BadRequest, result.ErrorTypes);
        Assert.Equal("You can only have two membership plans at the same time.", result.ErrorMessage);

        await _membershipPlanRepository
            .DidNotReceive()
            .AddAsync(Arg.Any<Domain.Aggregates.MembershipPlan.MembershipPlan>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenPlanIsCreated()
    {
        // Arrange
        var input = CreateValidInput();

        _membershipPlanRepository
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(new List<Domain.Aggregates.MembershipPlan.MembershipPlan>());

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal("Gold Plan", result.Value.Name);
        Assert.Equal(199m, result.Value.Price);

        await _membershipPlanRepository
            .Received(1)
            .AddAsync(Arg.Any<Domain.Aggregates.MembershipPlan.MembershipPlan>(), Arg.Any<CancellationToken>());

        await _unitOfWork
            .Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }

}
