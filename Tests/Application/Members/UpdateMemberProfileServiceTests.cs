
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction;
using Application.Common.Results;
using Application.Members.Inputs;
using Application.Members.Services;
using Domain.Abstractions.Repositories.Members;
using Domain.Aggregates.Members;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Tests.Application.Members;

public class UpdateMemberProfileServiceTests
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateMemberProfileService _service;

    public UpdateMemberProfileServiceTests()
    {
        _memberRepository = Substitute.For<IMemberRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _service = new UpdateMemberProfileService(
            _memberRepository,
            _unitOfWork);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenInputIsNull()
    {
        // Act
        var result = await _service.ExecuteAsync(null!);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.BadRequest, result.ErrorTypes);
        Assert.Equal("input must be provided.", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenMemberDoesNotExist()
    {
        // Arrange
        var input = new UpdateMemberProfileInput(
            "user-123",
            "John",
            "Doe",
            "0701234567",
            null
        );

        _memberRepository
            .GetMemberByUserIdAsync(input.UserId, Arg.Any<CancellationToken>())
            .Returns((Member?)null);

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
        var input = new UpdateMemberProfileInput(
            "user-123",
            "John",
            "Doe",
            "0701234567",
            null
        );

        var member = Member.Create("user-123");

        _memberRepository
            .GetMemberByUserIdAsync(input.UserId, Arg.Any<CancellationToken>())
            .Returns(member);

        _memberRepository
            .UpdateAsync(member, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenUpdateSucceeds()
    {
        // Arrange
        var input = new UpdateMemberProfileInput(
            "user-123",
            "John",
            "Doe",
            "0701234567",
            null
        );

        var member = Member.Create("user-123");

        _memberRepository
            .GetMemberByUserIdAsync(input.UserId, Arg.Any<CancellationToken>())
            .Returns(member);

        _memberRepository
            .UpdateAsync(member, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal("user-123", result.Value.UserId);

        await _memberRepository
            .Received(1)
            .UpdateAsync(member, Arg.Any<CancellationToken>());

        await _unitOfWork
            .Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var input = new UpdateMemberProfileInput(
            "user-123",
            "John",
            "Doe",
            "0701234567",
            null
        );

        _memberRepository
            .GetMemberByUserIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Throws(new Exception("unexpected error"));

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal("unexpected error", result.ErrorMessage);
    }
}
