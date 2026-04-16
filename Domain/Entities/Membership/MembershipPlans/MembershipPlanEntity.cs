namespace Domain.Entities.Membership.MembershipPlan;

public class MembershipPlanEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public decimal Price { get; private set; }
    public int ValidDays { get; private set; }


    public virtual ICollection<MembershipEntity> Memberships { get; private set; } = new List<MembershipEntity>();


    private MembershipPlanEntity() { }

    public MembershipPlanEntity(string name, string description, decimal price, int validDays)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        ValidDays = validDays;
    }

    public MembershipPlanEntity(Guid id,string name, string description, decimal price, int validDays)
    {
        Id = id;
        Name = name;
        Description = description;
        Price= price;
        ValidDays= validDays;
    }

    public void UpdateInformation(string name, string description, decimal price, int validDays)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty");

        if (price < 0)
            throw new ArgumentException("Price cannot be negative");

        if (validDays < 0)
            throw new ArgumentException("Valid days cannot be negative");

        Name = name.Trim();
        Description = description.Trim();
        Price = price;
        ValidDays = validDays;
    }
}
