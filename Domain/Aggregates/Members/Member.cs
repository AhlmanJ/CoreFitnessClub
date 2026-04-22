using System.Text.RegularExpressions;

namespace Domain.Aggregates.Members;

public class Member
{
    public Guid Id { get; private set; }

    public string UserId { get; private set; } = null!; // FK to ApplicationUser

    public string? FirstName { get; private set; }

    public string? LastName { get; private set;}

    public string? PhoneNumber { get; private set; }

    public string? ProfileImageUrl { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? ModifiedAt { get; private set; }

    private Member()
    {

    }

    private Member(Guid id, string userId, DateTimeOffset createdAt)
    {
        Id = id;
        UserId = userId;
        CreatedAt = createdAt;
    }

    public static Member Create(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("Application User id is required");

        var member = new Member(
            Guid.NewGuid(),
            userId,
            DateTimeOffset.UtcNow
        );

        return member;
    }


    // Rehydrate
    public static Member Rehydrate(Guid id, string userId, string? firstName, string? lastName, string? phoneNumber, string? profileImageUrl, DateTimeOffset createdAt, DateTimeOffset? modifiedAt)
    {
        var member = new Member(id, userId, createdAt)
        { 
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            ProfileImageUrl = profileImageUrl,
            ModifiedAt = modifiedAt
        };

        return member;
    }

    public void UpdateInformation(string? firstName, string? lastName, string? phoneNumber, string? profileImageUrl)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required");

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber;
        ModifiedAt = DateTimeOffset.UtcNow;
        if (!string.IsNullOrWhiteSpace(profileImageUrl))
        {
            ProfileImageUrl = profileImageUrl;
        }
    }

    public void ValidatePhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is required");

        var phoneNumberRegEx = @"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$";

        if (!Regex.IsMatch(phoneNumber, phoneNumberRegEx))
            throw new ArgumentException("Invalid phone number, must be a valid phone number with or whitout + at the beginning.");

    }
}
