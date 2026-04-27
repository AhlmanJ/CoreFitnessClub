
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction.MembershipReadInterface;
using Application.Common.Results;
using Application.Memberships.Outputs;
using Application.Memberships.Services;
using Domain.Abstractions.Repositories.Members;
using Domain.Aggregates.Members;
using NSubstitute;

namespace Tests.Application.Memberships;

public class GetMembershipByUserIdServiceTests
{
    private readonly IMembershipQueryService _membershipQueryService;
    private readonly IMemberRepository _memberRepository;
    private readonly GetMembershipByUserIdService _service;

    public GetMembershipByUserIdServiceTests()
    {
        _membershipQueryService = Substitute.For<IMembershipQueryService>();
        _memberRepository = Substitute.For<IMemberRepository>();

        _service = new GetMembershipByUserIdService(
            _membershipQueryService,
            _memberRepository
        );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenUserIdIsEmpty()
    {
        var result = await _service.ExecuteAsync("");

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.BadRequest, result.ErrorTypes);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenMemberDoesNotExist()
    {

        var userId = "user-1";

        _memberRepository
            .GetMemberByUserIdAsync(userId)
            .Returns((Member?)null);

        var result = await _service.ExecuteAsync(userId);

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.NotFound, result.ErrorTypes);
        Assert.Equal("Member not found.", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenMembershipIsMissing()
    {
        var userId = "user-1";

        var member = Member.Rehydrate(
            Guid.NewGuid(),
            userId,
            "John",
            "Doe",
            null,
            null,
            DateTimeOffset.UtcNow,
            null
        );

        _memberRepository
            .GetMemberByUserIdAsync(userId)
            .Returns(member);

        _membershipQueryService
            .GetMembershipByMemberIdAsync(member.Id, Arg.Any<CancellationToken>())
            .Returns((MembershipResponseOutput?)null);

        var result = await _service.ExecuteAsync(userId);

        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.BadRequest, result.ErrorTypes);
        Assert.Equal("Could not find an active membership for the requested member.", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenMembershipExists()
    {
        var userId = "user-1";

        var member = Member.Rehydrate(
            Guid.NewGuid(),
            userId,
            "John",
            "Doe",
            null,
            null,
            DateTimeOffset.UtcNow,
            null
        );

        var output = new MembershipResponseOutput(
            member.Id,
            "John",
            "Doe",
            "Gold",
            199,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddDays(30)
        );

        _memberRepository
            .GetMemberByUserIdAsync(userId)
            .Returns(member);

        _membershipQueryService
            .GetMembershipByMemberIdAsync(member.Id, Arg.Any<CancellationToken>())
            .Returns(output);

        var result = await _service.ExecuteAsync(userId);

        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal(member.Id, result.Value!.memberId);
    }
}
