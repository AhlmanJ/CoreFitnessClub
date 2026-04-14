
/*
 * Service-registration for the Infrastructure-layer.
 * I got help from chatGpt to explain that this is where i would implement Unit Of Work. In my last assignment, i implemented Unit Of Work in Program.cs
*/

using Application.Abstraction;
using Infrastructure.Extensions.Identity;
using Infrastructure.Identity;
using Infrastructure.Persistence.Extensions;
using Infrastructure.Persistence.UnitOfWork;
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

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
