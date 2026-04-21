namespace Presentation.WebApp.ViewModels;

public class MembershipViewModel
{
    public Guid MemberId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }

    public MembershipPlanViewModel MembershipPlan { get; set; } = null!;

    public List<MembershipListViewModel> Memberships { get; set; } = null!;
}
