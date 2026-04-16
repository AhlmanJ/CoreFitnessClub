namespace Application.MembershipPlans.Inputs;

public record UpdateMembershipPlanInput
(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int ValidDays
);