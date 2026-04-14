using Presentation.WebApp.Models.AccountModels;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.ViewModels.Account;

public class MyAccountViewModel 
{
    [Display(Name = "Email Address *")]
    public string Email { get; set; } = string.Empty;
    public MyProfileForm AboutMeForm { get; set; } = new();
}