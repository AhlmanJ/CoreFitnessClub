using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence;

public static class PersistenceDatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider sp, IHostEnvironment env, CancellationToken ct = default)
    {
        if (env.IsDevelopment())
        {
            using var scope = sp.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            await context.Database.EnsureCreatedAsync(ct);
        }
        else
        {
            using var scope = sp.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            await context.Database.MigrateAsync(ct);
        }
    }
}
