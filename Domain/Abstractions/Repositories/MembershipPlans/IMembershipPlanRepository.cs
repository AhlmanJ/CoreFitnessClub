using Domain.Persistence.Repositories;

namespace Domain.Abstractions.Repositories.MembershipPlans;

public interface IMembershipPlanRepository : IRepositoryBase<Aggregates.MembershipPlan.MembershipPlan, Guid>
{

}
