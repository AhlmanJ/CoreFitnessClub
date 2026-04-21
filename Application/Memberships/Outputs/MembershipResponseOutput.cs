namespace Application.Memberships.Outputs;

public record MembershipResponseOutput
(
    Guid memberId,
    String FirstName,
    String LastName,
    String Name,
    Decimal Price,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate
);
