using Application.Abstraction.MembershipInterface;
using Application.Memberships.Services;
using Microsoft.Extensions.DependencyInjection;


namespace Application.Extensions;

public static class MembershipRegistrationExtension
{ 
    public static IServiceCollection AddMembershipServices(this IServiceCollection services)
    {
        services.AddScoped<IEnrollMembershipService, EnrollMembershipService>();
        services.AddScoped<IGetAllMembershipsService, GetAllMembershipsService>();
        services.AddScoped<IGetMembershipByUserIdService, GetMembershipByUserIdService>();
        services.AddScoped<IDeleteMembershipByMemberIdService, DeleteMembershipByMemberIdService>();

        return services;
    }
}
