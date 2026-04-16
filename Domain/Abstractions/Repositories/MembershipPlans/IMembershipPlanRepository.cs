using Domain.Aggregates.MembershipPlan;
using Domain.Persistence.Repositories;

namespace Domain.Abstractions.Repositories.MembershipPlans;

public interface IMembershipPlanRepository : IRepositoryBase<MembershipPlan, Guid>
{
}
