
// Service where you add Repositories.

using Application.Abstraction.MembershipReadInterface;
using Domain.Abstractions.Repositories.Members;
using Domain.Abstractions.Repositories.MembershipPlans;
using Domain.Abstractions.Repositories.Memberships;
using Infrastructure.Persistence.Repositories.MembershipPlanRepo;
using Infrastructure.Persistence.Repositories.MembershipRepos;
using Infrastructure.Persistence.Repositories.MembersRepo;
using Infrastructure.QueryServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence.Repositories.Extensions;

public static class RepositoryRegistrationExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IMembershipPlanRepository, MembershipPlanRepository>();
        services.AddScoped<IMembershipRepository, MembershipRepository>();

        services.AddScoped<IMembershipQueryService, MembershipQueryService>();

        return services;
    }
}
