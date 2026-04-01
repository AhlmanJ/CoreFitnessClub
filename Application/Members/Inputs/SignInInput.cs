namespace Application.Members.Inputs;

public record SignInInput
(
    string Email,
    string Password,
    bool RememberMe
);
