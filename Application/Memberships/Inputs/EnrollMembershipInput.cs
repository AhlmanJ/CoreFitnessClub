namespace Application.Memberships.Inputs;

public record EnrollMembershipInput
(
    string UserId,
    Guid MembershipPlanId
);
