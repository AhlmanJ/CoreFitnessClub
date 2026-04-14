using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.ViewModels.CustomerService;

public class ContactFormViewModel
{
    [Display(Name = "First name", Prompt = " Enter First name")]
    [Required(ErrorMessage = "Required")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last name", Prompt = " Enter Last name")]
    [Required(ErrorMessage = "Required")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email", Prompt = " Enter Email")]
    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid Email, use name@example.com")]
    public string Email { get; set; } = null!;

    [Display(Name = "Phone number", Prompt = " Enter Phone number")]
    [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$", ErrorMessage = "Invalid Phone number")]
    public string? Phonenumber { get; set; }

    [Display(Name = "Message", Prompt = "Message...")]
    [Required(ErrorMessage = "Required")]
    public string Message { get; set; } = null!;
}
