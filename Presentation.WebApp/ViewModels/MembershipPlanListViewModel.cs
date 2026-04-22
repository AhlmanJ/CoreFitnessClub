namespace Presentation.WebApp.ViewModels;

public class MembershipPlanListViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ListItem1 { get; set; } = null!;
    public string ListItem2 { get; set; } = null!;
    public string ListItem3 { get; set; } = null!;
    public decimal Price { get; set; }
    public int ValidDays { get; set; }
}
