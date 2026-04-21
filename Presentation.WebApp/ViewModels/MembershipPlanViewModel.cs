using Presentation.WebApp.Models.MembershipPlanModels;

namespace Presentation.WebApp.ViewModels;

public class MembershipPlanViewModel
{
    public Guid Id { get; set; }
    public MembershipPlanForm Form { get; set; } = new();
    public List<MembershipPlanListViewModel> Plans { get; set; } = new();
}
