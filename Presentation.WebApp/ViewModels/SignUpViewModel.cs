using Presentation.WebApp.Models.AuthenticationModels;

namespace Presentation.WebApp.ViewModels;

public class SignUpViewModel
{
    public string? ReturnUrl { get; set; } = null!;
    public List<string> ExternalProviders { get; set; } = [];
    public RegisterEmailForm Form { get; set; } = new();
}
