using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence.Contexts.Extensions;

public static class ContextRegistrationExtension
{
    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        
        return services;
    }
}
