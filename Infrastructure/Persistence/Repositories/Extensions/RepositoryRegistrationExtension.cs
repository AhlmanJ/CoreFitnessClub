
// Service where you add Repositories.

using Domain.Abstractions.Repositories.Members;
using Infrastructure.Persistence.Repositories.MembersRepo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence.Repositories.Extensions;

public static class RepositoryRegistrationExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddScoped<IMemberRepository, MemberRepository>();

        return services;
    }
}
