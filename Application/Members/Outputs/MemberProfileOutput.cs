namespace Application.Members.Outputs;

public record MemberProfileOutput
(
    Guid Id,
    string UserId,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? ProfileImageUrl,
    DateTimeOffset? CreatedAt,
    DateTimeOffset? ModifiedAt
);
