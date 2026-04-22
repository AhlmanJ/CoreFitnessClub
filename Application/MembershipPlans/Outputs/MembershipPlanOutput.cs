namespace Application.MembershipPlans.Outputs;

public record MembershipPlanOutput
(
    Guid Id,
    string Name,
    string Description,
    string ListItem1,
    string ListItem2,
    string ListItem3,
    decimal Price,
    int ValidDays
);
