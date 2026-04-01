namespace Application.Members.Inputs;

public record UpdateMemberProfileInput
(
    string UserId,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? ProfileImageUrl
);