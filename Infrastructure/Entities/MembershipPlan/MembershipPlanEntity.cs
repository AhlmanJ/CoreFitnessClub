using Infrastructure.Entities.Membership;

namespace Infrastructure.Entities.MembershipPlan;

public class MembershipPlanEntity
{
    public Guid Id { get; internal set; }
    public string Name { get; internal set; } = null!;
    public string Description { get; internal set; } = null!;
    public string ListItem1 { get; internal set; } = null!;
    public string ListItem2 { get; internal set; } = null!;
    public string ListItem3 { get; internal set; } = null!;
    public decimal Price { get; internal set; }
    public int ValidDays { get; internal set; }
    public byte[] RowVersion { get; private set; } = null!;

    public virtual ICollection<MembershipEntity> Memberships { get; private set; } = new List<MembershipEntity>();


    private MembershipPlanEntity() { }

     public MembershipPlanEntity(Guid id,string name, string description, string listItem1, string listItem2, string listItem3, decimal price, int validDays)
    {
        Id = id;
        Name = name;
        Description = description;
        ListItem1 = listItem1;
        ListItem2 = listItem2;
        ListItem3 = listItem3;
        Price= price;
        ValidDays= validDays;
    }
}
