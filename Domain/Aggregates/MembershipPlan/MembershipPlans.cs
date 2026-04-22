namespace Domain.Aggregates.MembershipPlan;

public class MembershipPlan
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string ListItem1 { get; internal set; } = null!;
    public string ListItem2 { get; internal set; } = null!;
    public string ListItem3 { get; internal set; } = null!;
    public decimal Price { get; private set; }
    public int ValidDays { get; private set; } = 0;

    private MembershipPlan() 
    { 
    
    }

    private MembershipPlan(Guid id, string name, string description, string listItem1, string listItem2, string listItem3, decimal price, int validDays)
    {
        Id = id;
        Name = name;
        Description = description;
        ListItem1 = listItem1;
        ListItem2 = listItem2;
        ListItem3 = listItem3;
        Price = price;
        ValidDays = validDays;
    }

    public static MembershipPlan Create(string name, string description, string listItem1, string listItem2, string listItem3, decimal price, int validDays)
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
            Guid.NewGuid(),
            name,
            description,
            listItem1,
            listItem2,
            listItem3,
            price,
            validDays
        );
    }

    public static MembershipPlan Rehydrate(Guid id, string name, string description, string listItem1, string listItem2, string listItem3, decimal price, int validDays)
    {
        return new MembershipPlan
            (
                id,
                name,
                description,
                listItem1,
                listItem2,
                listItem3,
                price,
                validDays
            );
    }

    public void UpdateMembershipPlan(string name, string description, string listItem1, string listItem2, string listItem3, decimal price, int validDays)
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
        ListItem1 = listItem1.Trim();
        ListItem2 = listItem2.Trim();
        ListItem3 = listItem3.Trim();
        Price = price;
        ValidDays = validDays;
    }
}
