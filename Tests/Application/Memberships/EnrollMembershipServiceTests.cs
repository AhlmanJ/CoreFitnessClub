
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction;
using Application.Common.Results;
using Application.Memberships.Inputs;
using Application.Memberships.Services;
using Domain.Abstractions.Repositories.Members;
using Domain.Abstractions.Repositories.MembershipPlans;
using Domain.Abstractions.Repositories.Memberships;
using Domain.Aggregates.Members;
using Domain.Aggregates.MembershipPlan;
using Domain.Aggregates.Memberships;
using NSubstitute;

namespace Tests.Application.Memberships;

public class EnrollMembershipServiceTests
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMembershipPlanRepository _membershipPlanRepository;
    private readonly IUnitOfWork _unitOfWork;

    private readonly EnrollMembershipService _service;

    public EnrollMembershipServiceTests()
    {
        _membershipRepository = Substitute.For<IMembershipRepository>();
        _memberRepository = Substitute.For<IMemberRepository>();
        _membershipPlanRepository = Substitute.For<IMembershipPlanRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _service = new EnrollMembershipService(
            _membershipRepository,
            _memberRepository,
            _membershipPlanRepository,
            _unitOfWork
        );
    }

    private EnrollMembershipInput CreateInput() => new("user-123", Guid.NewGuid());

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenInputIsNull()
    {
        var result = await _service.ExecuteAsync(null!);

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.BadRequest, result.ErrorTypes);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenMemberNotFound()
    {
        var input = CreateInput();

        _memberRepository
            .GetMemberByUserIdAsync(input.UserId)
            .Returns((Domain.Aggregates.Members.Member?)null);

        var result = await _service.ExecuteAsync(input);

        Assert.False(result.Success);
        Assert.Equal("Could not find the member.", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenFirstNameIsNull()
    {
        var input = CreateInput();

        var member = Domain.Aggregates.Members.Member.Rehydrate(
            Guid.NewGuid(),
            input.UserId,
            null,   // FirstName
            "Doe",  // LastName (eller null beroende test)
            null,
            null,
            DateTimeOffset.UtcNow,
            null
        );

        _memberRepository
            .GetMemberByUserIdAsync(input.UserId)
            .Returns(member);

        var result = await _service.ExecuteAsync(input);

        Assert.False(result.Success);
        Assert.Equal("First name not found.", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenMembershipAlreadyExists()
    {
        var input = CreateInput();

        var member = Member.Rehydrate( // Creates objects with methods in Aggregates in the Domain layer.
            Guid.NewGuid(),
            input.UserId,
            "John",
            "Doe",
            null,
            null,
            DateTimeOffset.UtcNow,
            null
        );

        var membership = Membership.Rehydrate( // Creates objects with methods in Aggregates in the Domain layer.
            Guid.NewGuid(),
            member.Id,
            input.MembershipPlanId,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddDays(30)
        );

        _memberRepository
            .GetMemberByUserIdAsync(input.UserId)
            .Returns(member);

        _membershipRepository
            .GetByMemberIdAsync(member.Id, Arg.Any<CancellationToken>())
            .Returns(membership);

        var result = await _service.ExecuteAsync(input);

        Assert.False(result.Success);
        Assert.Equal("You already have an existing membership.", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenPlanNotFound()
    {
        var input = CreateInput();

        var member = Member.Rehydrate(
            Guid.NewGuid(),
            input.UserId,
            "John",
            "Doe",
            null,
            null,
            DateTimeOffset.UtcNow,
            null
        );

        _memberRepository
            .GetMemberByUserIdAsync(input.UserId)
            .Returns(member);

        _membershipRepository
            .GetByMemberIdAsync(member.Id, Arg.Any<CancellationToken>())
            .Returns((Membership?)null);

        _membershipPlanRepository
            .GetByIdAsync(input.MembershipPlanId, Arg.Any<CancellationToken>())
            .Returns((Domain.Aggregates.MembershipPlan.MembershipPlan?)null);

        var result = await _service.ExecuteAsync(input);

        Assert.False(result.Success);
        Assert.Equal("Plan not found.", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenMembershipIsCreated()
    {
        var input = CreateInput();

        var memberId = Guid.NewGuid();

        var member = Member.Rehydrate(
            Guid.NewGuid(),
            input.UserId,
            "John",
            "Doe",
            null,
            null,
            DateTimeOffset.UtcNow,
            null
        );

        var plan = MembershipPlan.Rehydrate(
            input.MembershipPlanId,
            "Gold",
            "Description",
            "A",
            "B",
            "C",
            199m,
            30
        );

        _memberRepository
            .GetMemberByUserIdAsync(input.UserId)
            .Returns(member);

        _membershipRepository
            .GetByMemberIdAsync(memberId, Arg.Any<CancellationToken>())
            .Returns((Membership?)null);

        _membershipPlanRepository
            .GetByIdAsync(input.MembershipPlanId, Arg.Any<CancellationToken>())
            .Returns(plan);

        var result = await _service.ExecuteAsync(input);

        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal("John", result.Value.FirstName);

        await _membershipRepository
            .Received(1)
            .AddAsync(Arg.Any<Membership>(), Arg.Any<CancellationToken>());

        await _unitOfWork
            .Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        var input = CreateInput();

       _memberRepository
            .GetMemberByUserIdAsync(input.UserId)!
            .Returns(Task.FromException<Domain.Aggregates.Members.Member>( // When "GetMemberByUserIdAsync" is called, it returns a Task that will throw an Exception when awaited.
                new Exception("DB error")));

        var result = await _service.ExecuteAsync(input);

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal("DB error", result.ErrorMessage);
    }
}
