namespace Application.MembershipPlans.Inputs;

public record UpdateMembershipPlanInput
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