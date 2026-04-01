using Application.Abstraction;
using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions.Identity;

public static class IdentityRegistrationExtention
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequiredLength = 8;  // Set password length.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;

            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedAccount = false;

        })
        .AddEntityFrameworkStores<DataContext>()
        .AddDefaultTokenProviders();

        // Configurate Cookie settings:
       services.ConfigureApplicationCookie(options => 
        {
            options.LoginPath = "/authentication/sign-in";
            options.AccessDeniedPath = "/authentication/sign-in";
            options.Cookie.Name = "CoreFitness.Auth";
        });

        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}