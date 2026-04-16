namespace Presentation.WebApp.ViewModels;

public class AdminViewModel
{
    public string? Section { get; set; }

    public MembershipPlanViewModel? CreateMembershipPlan { get; set; } = new MembershipPlanViewModel();
}
