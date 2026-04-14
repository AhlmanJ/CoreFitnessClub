
/*
 * I got help from chatGPT on how to do this IdentitySeeder. I Searched online for instructions but didn't find any good info about this.
 * The information i found was general information about how Roles work.
 */ 

using Application.Common.Roles;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var config = serviceProvider.GetRequiredService<IConfiguration>();

        string[] roles = { "Admin", "User", "Trainer" };

        foreach (var role in roles) 
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var adminEmail = config["SeedAdmin:Email"];
        var adminPassword = config["SeedAdmin:Password"];

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            return;

        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpper(),
                NormalizedUserName = adminEmail.ToUpper(),
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, adminPassword);
            if(!result.Succeeded)
            {
                var errors = string.Join(",",result.Errors.Select(e => e.Description)); // Converts errors to text.
                throw new Exception($"Failed to create admin: {errors}");
            }
            else
            {
                await userManager.AddToRoleAsync(admin, ApplicationRoles.Admin);
            }
            
        }
        else
        {
            if(!await userManager.IsInRoleAsync(admin, ApplicationRoles.Admin))
            {
                await userManager.AddToRoleAsync(admin, ApplicationRoles.Admin);
            }
        }
    }
}
