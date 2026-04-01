using Application.Common.Results;

namespace Application.Abstraction;

public interface IIdentityService
{
    Task<Result<string?>> CreateUserAsync(string email, string password, CancellationToken ct = default, bool confirmedEmail = true, string? firstName = null, string? lastName = null, string? phoneNumber = null, string? profileImageUrl = null);
    Task<Result<string?>> PasswordSignInAsync(string email, string password, bool rememberMe, CancellationToken ct = default);
    Task SignOutAsync(CancellationToken ct = default);
    Task<Result<string?>> DeleteUserAsync(string email, CancellationToken ct = default);
}
