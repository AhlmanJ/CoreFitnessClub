
// Service-registration for the Infrastructure-layer.

using Infrastructure.Extensions.Identity;
using Infrastructure.Identity;
using Infrastructure.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Extensions;

public static class InfrastructureServiceCollectionRegistrationExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddPersistence(configuration, env);
        services.AddIdentityServices();

        services.AddTransient<ApplicationUserFactory>();

        return services;
    }
}
