namespace Domain.Aggregates.MembershipPlan;

public class MembershipPlan
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public decimal Price { get; private set; }
    public int ValidDays { get; private set; }

    private MembershipPlan() 
    { 
    
    }

    private MembershipPlan(Guid id, string name, string description, decimal price, int validDays)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        ValidDays = validDays;
    }

    public static MembershipPlan Create(Guid id, string name, string description, decimal price, int validDays)
    {
        if(string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Name is required");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required");
        if (price < 0)
            throw new ArgumentException("Price cannot be negative");
        if (validDays < 0)
            throw new ArgumentException("Valid days cannot be negative");

        return new MembershipPlan
        (
            id,
            name,
            description,
            price,
            validDays
        );
    }

    public void UpdateMembershipPlan(string name, string description, decimal price, int validDays)
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
