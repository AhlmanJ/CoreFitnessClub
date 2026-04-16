namespace Application.MembershipPlans.Outputs;

public record MembershipPlanOutput
(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int ValidDays
);
