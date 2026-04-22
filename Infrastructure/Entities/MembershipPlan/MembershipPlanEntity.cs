using Infrastructure.Entities.Membership;

namespace Infrastructure.Entities.MembershipPlan;

public class MembershipPlanEntity
{
    public Guid Id { get; internal set; }
    public string Name { get; internal set; } = null!;
    public string Description { get; internal set; } = null!;
    public decimal Price { get; internal set; }
    public int ValidDays { get; internal set; }


    public virtual ICollection<MembershipEntity> Memberships { get; private set; } = new List<MembershipEntity>();


    private MembershipPlanEntity() { }

     public MembershipPlanEntity(Guid id,string name, string description, decimal price, int validDays)
    {
        Id = id;
        Name = name;
        Description = description;
        Price= price;
        ValidDays= validDays;
    }
}
