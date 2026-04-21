namespace Application.Memberships.Inputs;

public record UpdateMembershipInput
(
    Guid MemberId,
    Guid MembershipPlanId
);
