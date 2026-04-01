
// NOTE! ChatGPT helped med with this. ( It was the first entity i created so i needed help with an explanation of how to work with the code. )

using Domain.Entities.Members;

namespace Infrastructure.Factories;

public class MemberEntityFactory
{

    // To create a new MemberEntity.
    public MemberEntity Create(string userId, string? firstName, string? lastName, string? phoneNumber = null, string? profileImageUrl = null)
    {
        return new MemberEntity(userId, firstName, lastName, phoneNumber, profileImageUrl);
    }

    // To update a MemberENtity
    public MemberEntity Update(string id, string userId, string? firstName, string? lastName, string? phoneNumber, string? profileImageUrl, DateTimeOffset createdAt, DateTimeOffset? modifiedAt)
    {
        return new MemberEntity(id, userId, firstName, lastName, phoneNumber, profileImageUrl, createdAt, modifiedAt);
    }
}
