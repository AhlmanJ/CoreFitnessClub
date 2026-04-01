using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.AuthenticationModels;

public class SignInForm
{
    [Required(ErrorMessage = "Email adress is required")]
    [Display(Name = "Email Address *", Prompt = "Enter Email Adress")]
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid Email, use name@example.com")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter Password")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$", ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, number and special character")]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }

    [Required(ErrorMessage = "You must accept the terms and conditions.")]
    public bool AcceptTerms {  get; set; }
}