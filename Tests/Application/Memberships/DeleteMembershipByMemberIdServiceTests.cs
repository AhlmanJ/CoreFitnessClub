
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction;
using Application.Common.Results;
using Application.Memberships.Services;
using Domain.Abstractions.Repositories.Memberships;
using NSubstitute;

namespace Tests.Application.Memberships;

public class DeleteMembershipByMemberIdServiceTests
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteMembershipByMemberIdService _service;

    public DeleteMembershipByMemberIdServiceTests()
    {
        _membershipRepository = Substitute.For<IMembershipRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _service = new DeleteMembershipByMemberIdService(
            _membershipRepository,
            _unitOfWork
        );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnBadRequest_WhenMemberIdIsEmpty()
    {
        // Arrange
        var memberId = Guid.Empty;

        // Act
        var result = await _service.ExecuteAsync(memberId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.BadRequest, result.ErrorTypes);

        await _membershipRepository
            .DidNotReceive()
            .RemoveByMemberIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenMembershipDoesNotExist()
    {
        // Arrange
        var memberId = Guid.NewGuid();

        _membershipRepository
            .RemoveByMemberIdAsync(memberId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false));

        // Act
        var result = await _service.ExecuteAsync(memberId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.NotFound, result.ErrorTypes);

        await _unitOfWork.DidNotReceive()
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenMembershipIsDeleted()
    {
        // Arrange
        var memberId = Guid.NewGuid();

        _membershipRepository
            .RemoveByMemberIdAsync(memberId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        // Act
        var result = await _service.ExecuteAsync(memberId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Membership was deleted successfully.", result.Value);

        await _unitOfWork.Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var memberId = Guid.NewGuid();

        _membershipRepository
            .RemoveByMemberIdAsync(memberId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<bool>(new Exception("DB error")));

        // Act
        var result = await _service.ExecuteAsync(memberId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal("DB error", result.ErrorMessage);
    }
}
