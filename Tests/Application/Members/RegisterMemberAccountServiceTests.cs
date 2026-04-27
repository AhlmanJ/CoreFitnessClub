
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

public class RegisterMemberAccountServiceTests
{
    private readonly IIdentityService _identityService;
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RegisterMemberAccountService _service;

    public RegisterMemberAccountServiceTests()
    {
        _identityService = Substitute.For<IIdentityService>();
        _memberRepository = Substitute.For<IMemberRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _service = new RegisterMemberAccountService(
            _identityService,
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
        Assert.Equal("The input field was empty.", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenIdentityCreationFails()
    {
        // Arrange
        var input = new RegisterMemberAccountInput(
            "test@test.com",
            "password123"
        );

        _identityService
            .CreateUserAsync(input.Email, input.Password, Arg.Any<CancellationToken>())!
            .Returns(Result<string>.InternalServerError("Identity failed")); // Returns internal server error.

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal("Identity failed", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenUserIdIsEmpty()
    {
        // Arrange
        var input = new RegisterMemberAccountInput(
            "test@test.com",
            "password123"
        );

        _identityService
            .CreateUserAsync(input.Email, input.Password, Arg.Any<CancellationToken>())!
            .Returns(Result<string>.Ok("")); // invalid

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenRegistrationSucceeds()
    {
        // Arrange
        var input = new RegisterMemberAccountInput(
            "test@test.com",
            "password123"
        );

        var userId = "user-123"; // IdentityService should return userId if a user is created.

        _identityService
            .CreateUserAsync(input.Email, input.Password, Arg.Any<CancellationToken>())!
            .Returns(Result<string>.Ok(userId));

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Equal(userId, result.Value.UserId);

        await _memberRepository // Assert that repository save a new member in the database.
            .Received(1)
            .AddAsync(Arg.Any<Member>(), Arg.Any<CancellationToken>());

        await _unitOfWork // Assert that changes is been saved in the database.
            .Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var input = new RegisterMemberAccountInput(
            "test@test.com",
            "password123"
        );

        _identityService
            .CreateUserAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Throws(new Exception("Unexpected error"));  // Throws Exception.

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal("Unexpected error", result.ErrorMessage);
    }
}
