using Domain.Abstractions.Repositories.MembershipPlans;
using Domain.Aggregates.MembershipPlan;
using Domain.Entities.Membership.MembershipPlan;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Repositories.MembershipPlanRepo;

public class MembershipPlanRepository(DataContext context) : RepositoryBase<MembershipPlan, Guid, MembershipPlanEntity, DataContext>(context), IMembershipPlanRepository
{
    protected override Guid GetId(MembershipPlan model)
    {
        return model.Id;
    }

    protected override void UpdateEntity(MembershipPlanEntity entity, MembershipPlan model)
    {
        entity.UpdateInformation
            (
                model.Name,
                model.Description,
                model.Price,
                model.ValidDays
            );
    }

    protected override MembershipPlan ToDomainModel(MembershipPlanEntity entity)
    {
        var model = MembershipPlan.Create
            (
                entity.Id,
                entity.Name,
                entity.Description,
                entity.Price,
                entity.ValidDays
            );

        return model;
    }

    protected override MembershipPlanEntity ToEntity(MembershipPlan model)
    {
        var entity = new MembershipPlanEntity
            (
                model.Id,
                model.Name,
                model.Description,
                model.Price,
                model.ValidDays
            );

        return entity;
    }
}
