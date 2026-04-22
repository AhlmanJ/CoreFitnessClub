using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.AccountModels;

public class MyProfileForm
{
    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "First Name", Prompt = "Enter First Name")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required")]
    [Display(Name = "Last Name", Prompt = "Enter Last Name")]
    public string LastName { get; set; } = null!;

    [Phone]
    [RegularExpression(@"^\+?[0-9\s\-().]{7,20}$", ErrorMessage ="Invalid Phone number")]
    [Display(Name = "Phone Number", Prompt = "Enter Phone Number")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Profile Image", Prompt = "Upload Profile Image")]
    public string? ProfileImageUrl { get; set; }

    [Display(Name = "Upload Profile Image")]
    public IFormFile? File { get; set; }
}
