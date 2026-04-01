
// Service-registration for all the Persistance-related services.

using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence.Extensions;

public static class PersistenceRegistrationExtension
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddDbContexts(configuration, env);
        services.AddRepositories(configuration, env);

        return services;
    }
}
