
// ----- >    NOTE! The tests are created by chatGPT   < ----
// ----- > I have asked chatGPT to explain the code to me < ----

using Application.Abstraction;
using Application.Common.Results;
using Application.Members.Inputs;
using Application.Members.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Tests.Application.Members;

public class SignInMemberServiceTests
{
    private readonly IIdentityService _identityService;
    private readonly SignInMemberService _service;

    public SignInMemberServiceTests()
    {
        _identityService = Substitute.For<IIdentityService>();
        _service = new SignInMemberService(_identityService);
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
    public async Task ExecuteAsync_ShouldReturnUnauthorized_WhenLoginFails()
    {
        // Arrange
        var input = new SignInInput(
            "test@test.com",
            "wrongpassword",
            false
        );

        _identityService
            .PasswordSignInAsync(input.Email, input.Password, input.RememberMe, Arg.Any<CancellationToken>())
            .Returns(Result<string?>.Unauthorized("Invalid email or password"));

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.Unauthorized, result.ErrorTypes); // Verify that failed login returns Unauthorized from service layer.
        Assert.Equal("invalid email or password", result.ErrorMessage); // Verify correct error message is returned for failed login.
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenLoginSucceeds()
    {
        // Arrange
        var input = new SignInInput(
            "test@test.com",
            "password123",
            true
        );

        _identityService
            .PasswordSignInAsync(input.Email, input.Password, input.RememberMe, Arg.Any<CancellationToken>())
            .Returns(Result<string?>.Ok("Login succeeded"));

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("login succeeded", result.Value);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var input = new SignInInput(
            "test@test.com",
            "password123",
            true
        );

        _identityService
            .PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<CancellationToken>()) // Ignore input values and focus on authentication result.
            .Throws(new Exception("something went wrong"));

        // Act
        var result = await _service.ExecuteAsync(input);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorTypes.InternalServerError, result.ErrorTypes);
        Assert.Equal("something went wrong", result.ErrorMessage);
    }
}
