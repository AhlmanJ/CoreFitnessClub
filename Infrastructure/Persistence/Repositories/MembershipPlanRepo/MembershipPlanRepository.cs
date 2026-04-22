using Domain.Abstractions.Repositories.MembershipPlans;
using Domain.Aggregates.MembershipPlan;
using Infrastructure.Entities.MembershipPlan;
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

        entity.Name = model.Name;
        entity.Description = model.Description;
        entity.Price = model.Price;
        entity.ValidDays = model.ValidDays;
    }

    protected override MembershipPlan ToDomainModel(MembershipPlanEntity entity)
    {
        var model = MembershipPlan.Rehydrate
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
