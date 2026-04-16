namespace Presentation.WebApp.Models.MembershipPlanModels
{
    public class MembershipPlan
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int ValidDays { get; set; }

        public MembershipPlan() { }
    }
}
