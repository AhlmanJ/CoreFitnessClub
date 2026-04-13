
// Help from chatGPT - see row 37.

using Domain.Abstractions.Repositories.Members;
using Domain.Aggregates.Members;
using Domain.Entities.Members;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.MembersRepo;

public class MemberRepository(DataContext context) : RepositoryBase<Member, Guid, MemberEntity, DataContext>(context), IMemberRepository
{
    public async Task<Member?> GetMemberByUserIdAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            var entity = await Set.FirstOrDefaultAsync(x => x.UserId == userId,ct);
            return entity is null ? default : ToDomainModel(entity);
        }
        catch
        {
            throw;
        }
    }

    protected override Guid GetId(Member model)
    {
        return model.Id;
    }

    public string GetUserId(Member model)
    {
        return model.UserId;
    }

    // I got help from chatGPT on how to update the Entity while keeping the properites in the entity as "Private Set" to "protect" the entity.
    protected override void UpdateEntity(MemberEntity entity, Member model)
    {
        entity.UpdateInformation
            (
                model.FirstName,
                model.LastName,
                model.PhoneNumber,
                model.ProfileImageUrl
            );
    }

    protected override Member ToDomainModel(MemberEntity entity)
    {
        var model = Member.Create(
            entity.Id,
            entity.UserId,
            entity.FirstName,
            entity.LastName,
            entity.PhoneNumber,
            entity.ProfileImageUrl,
            entity.CreatedAt,
            entity.ModifiedAt
        );

        return model;
    }

    protected override MemberEntity ToEntity(Member model)
    {
        var entity = new MemberEntity
        (
            model.Id,
            model.UserId,
            model.FirstName,
            model.LastName,
            model.PhoneNumber,
            model.ProfileImageUrl,
            model.CreatedAt,
            model.ModifiedAt
        );

        return entity;
    }
}

