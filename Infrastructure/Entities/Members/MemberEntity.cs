
// NOTE! ChatGPT helped med with this. ( It was the first entity i created so i needed help with an explanation of how to work with the code. )

// Created with code and methods from ChatGpt.



// I made some changes:
/*
 * I had to compromise to simplify the mapping in the repository and change the "setters" from private to internal.
 * The difference is that the value of the parameters can be changed from methods throughout the Infrastructure layer and are therefore not completely "protected".
 * However, i choose this approach to simplyfy the mapping.
 * 
 */

using Infrastructure.Entities.Booking;
using Infrastructure.Entities.Membership;

namespace Infrastructure.Entities.Members;

public class MemberEntity
{
    public Guid Id { get; internal set; }

    public string UserId { get; internal set; } = null!; // FK to ApplicationUser

    public string? FirstName { get; internal set; }

    public string? LastName { get; internal set; }

    public string? PhoneNumber { get; internal set; }

    public string? ProfileImageUrl { get; internal set; }

    public byte[] RowVersion { get; private set; } = null!;

    public DateTimeOffset CreatedAt { get; internal set; }
    public DateTimeOffset? ModifiedAt { get; internal set; }

    public ICollection<BookingsEntity> Bookings { get; internal set; } = new List<BookingsEntity>();
    public ICollection<MembershipEntity> Memberships { get; internal set; } = new List<MembershipEntity>();

    private MemberEntity() { }

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
}