namespace Application.MembershipPlans.Inputs;

public record CreateMembershipPlanInput
(
    string Name,
    string Description,
    decimal Price,
    int ValidDays
);
