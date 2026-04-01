using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.AuthenticationModels;

public class RegisterEmailForm
{
    [Required(ErrorMessage = "Email adress is required")]
    [EmailAddress (ErrorMessage = "Invalid email address")]
    [Display(Name = "Email Address *", Prompt = "Enter Email Adress")]
    public string Email { get; set; } = null!;
}
