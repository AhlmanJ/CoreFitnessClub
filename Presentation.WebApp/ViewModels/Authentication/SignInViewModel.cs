using Presentation.WebApp.Models.AuthenticationModels;

namespace Presentation.WebApp.ViewModels.Authentication;

public class SignInViewModel
{
    public string? ReturnUrl { get; set; }

    public List<string> ExternalProviders { get; set; } = [];

    public SignInForm Form { get; set; } = new();
}
