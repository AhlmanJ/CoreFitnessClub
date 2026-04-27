using Presentation.WebApp.Models.AccountModels;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.ViewModels;

public class MyAccountViewModel 
{
    [Display(Name = "Email Address *")]
    public string Email { get; set; } = string.Empty;
    
    public MyProfileForm AboutMeForm { get; set; } = new();
    public MembershipViewModel? Membership { get; set; } = null!;

    public List<BookingsViewModel> Bookings { get; set; } = new();

}