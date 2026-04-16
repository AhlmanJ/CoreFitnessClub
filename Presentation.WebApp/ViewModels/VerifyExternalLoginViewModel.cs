using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.ViewModels;

public class VerifyExternalLoginViewModel
{
    public string Email { get; set; } = null!;
    public string? ReturnUrl { get; set; }

    [Required(ErrorMessage = " Verification code required! ")]
    public string VerificationCode { get; set; } = null!;

}
