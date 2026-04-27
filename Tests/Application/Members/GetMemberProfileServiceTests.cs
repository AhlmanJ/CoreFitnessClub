
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Common.Results;
using Application.Members.Services;
using Domain.Abstractions.Repositories.Members;
using Domain.Aggregates.Members;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Tests.Application.Members;

public class GetMemberProfileServiceTests
{
    private readonly IMemberRepository _memberRepository;
    private readonly GetMemberProfileService _service;

    public GetMemberProfileServiceTests()
    {
        _memberRepository = Substitute.For<IMemberRepository>();
        _service = new GetMemberProfileService(_memberRepository);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenUserIdIsInvalid(string userId)
    {
        // Act
        var result = await _service.ExecuteAsync(userId);

        // Assert
        Assert.False(result.Success); // The test should have failed.
        Assert.Equal(ErrorTypes.BadRequest, result.ErrorTypes); // The Error should be 404 Bad Request.
        Assert.Null(result.Value); // No data should be returned.
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenMemberExists()
    {
        // Arrange
        var userId = "123";
        var member = Member.Create(userId);

        _memberRepository
            .GetMemberByUserIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(member); // returns the "member" from the repository.

        // Act
        var result = await _service.ExecuteAsync(userId); // Service is called and returns member.

        // Assert
        Assert.True(result.Success); // The test should have been successful.
        Assert.Equal(member, result.Value); // Ensures that the result is the same as the requested object.
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenMemberDoesNotExist()
    {
        // Arrange
        var userId = "123";

        _memberRepository
            .GetMemberByUserIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns((Member?)null); // When userId is called, return "null".

        // Act
        var result = await _service.ExecuteAsync(userId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.NotFound, result.ErrorTypes);
        Assert.Null(result.Value);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var userId = "123";

        _memberRepository
            .GetMemberByUserIdAsync(userId, Arg.Any<CancellationToken>())
            .Throws(new Exception("DB failure")); // Throws Exception when userId is called.

        // Act
        var result = await _service.ExecuteAsync(userId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal("DB failure", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotCallRepository_WhenUserIdIsInvalid()
    {
        // Act
        await _service.ExecuteAsync("");

        // Assert
        await _memberRepository
            .DidNotReceive()
            .GetMemberByUserIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()); // Verifies that the repository is never called if an invalid input occurs.
    }
}
