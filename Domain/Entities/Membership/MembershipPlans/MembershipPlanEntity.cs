namespace Domain.Entities.Membership.MembershipPlan;

public class MembershipPlanEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public decimal Price { get; private set; }
    public int ValidDays { get; private set; }


    public virtual ICollection<MembershipEntity> Memberships { get; private set; } = new List<MembershipEntity>();
}
