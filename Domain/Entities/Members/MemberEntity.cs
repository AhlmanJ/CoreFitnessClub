
// NOTE! ChatGPT helped med with this. ( It was the first entity i created so i needed help with an explanation of how to work with the code. )

// User profile.

// Created with code and methods from ChatGpt.



// Private set to ensure that properties cannot be changed/modified after an object is created.

using Domain.Aggregates.Members;

namespace Domain.Entities.Members;

public class MemberEntity
{
    public Guid Id { get; private set; }

    public string UserId { get; private set; } = null!; // FK to ApplicationUser

    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }

    public string? PhoneNumber { get; private set; }

    public string? ProfileImageUrl { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ModifiedAt { get; private set; }


    private MemberEntity() { }

    // Constructor to create a MemberEntity.
    public MemberEntity(string userId, string? firstname, string? lastname, string? phoneNumber, string? profileImageUrl)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        FirstName = firstname;
        LastName = lastname;
        PhoneNumber = phoneNumber;
        ProfileImageUrl = profileImageUrl;
        CreatedAt = DateTimeOffset.UtcNow;
        ModifiedAt = null!; // No ModifiedAt the first time a Member is created.
    }

    // Constructor to update an already existing MemberEntity or create a MemberEntity with all properies. ( To update an existing Member )
    public MemberEntity(Guid id, string userId, string? firstname, string? lastname, string? phoneNumber, string? profileImageUrl, DateTimeOffset createdAt, DateTimeOffset? modifiedAt)
    {
        Id = id;
        UserId = userId;
        FirstName = firstname;
        LastName = lastname;
        PhoneNumber = phoneNumber;
        ProfileImageUrl = profileImageUrl;
        CreatedAt = createdAt;
        ModifiedAt = modifiedAt;
    }

    // Method to update the MemberEntity
    public void UpdateProfile(string? firstName, string? lastName, string? phoneNumber, string? profileImageUrl)
    {
        FirstName = firstName ?? FirstName; // If the new "firstName" is not null = Update FirstName with the new value. If "firstName" is null = keep the "old" value.
        LastName = lastName ?? LastName;
        PhoneNumber = phoneNumber ?? PhoneNumber;
        ProfileImageUrl = profileImageUrl ?? ProfileImageUrl;
        ModifiedAt = DateTimeOffset.UtcNow;
    }
}