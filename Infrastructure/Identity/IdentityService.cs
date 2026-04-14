
// NOTE! chatGPT
// I have taken help from the school lectures and chatGPT to fix the CreateUserAsync method.

using Application.Abstraction;
using Application.Common.Results;
using Application.Common.Roles;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationUserFactory applicationUserFactory) : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ApplicationUserFactory _applicationUserFactory = applicationUserFactory;

    // I got help from chatGPT on how to create and use a ApplicationUserFactory. ( The teacher mentioned in the lecture that it might be good to create factory for this, so i asked chatGPT to show how to do this. )
    public async Task<Result<string?>> CreateUserAsync(string email, string password, CancellationToken ct = default, bool confirmedEmail = true, string? firstName = null, string? lastName = null, string? phoneNumber = null, string? profileImageUrl = null)
    {
        var existingUser = await _userManager.FindByEmailAsync(email); // Uses one of the Identity package's own methods.
        if (existingUser != null)
            return Result<string?>.Conflict($"An account with the Email address {email} already exists");

        var user = _applicationUserFactory.Create(email, confirmedEmail); // Create a Application user.

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return Result<string?>.BadRequest("User creation failed.");

        var roleResult = await _userManager.AddToRoleAsync(user, ApplicationRoles.User); // All new users are given the "User" role by default when they register an account.
        if (!roleResult.Succeeded)
            return Result<string?>.BadRequest("User created, but failed to assign role.");

        return Result<string?>.Ok(user.Id);
    }

    public async Task<Result<string?>> PasswordSignInAsync(string email, string password, bool rememberMe, CancellationToken ct = default)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, false);

        if (!result.Succeeded)
            return Result<string?>.Unauthorized("Invalid email or password");

        return Result<string?>.Ok("Login succeeded");

    }

    public Task SignOutAsync(CancellationToken ct = default)
    {
        return _signInManager.SignOutAsync();
    }

    public async Task<Result<string?>> DeleteUserAsync(string email, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return Result<string?>.NotFound($"User email address: {email}, was not found. ");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return Result<string?>.BadRequest($"The user {user} was not deleted, please try again.");
       
        return Result<string?>.Ok("The user was deleted.");
    }
}
