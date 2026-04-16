using Application.Abstraction.MembershipPlansInterface;
using Application.MembershipPlans.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class MembershipPlanRegistrationExtention
{
    public static IServiceCollection AddMembershipPlanService(this IServiceCollection services)
    {
        services.AddScoped<ICreateMembershipPlanService, CreateMembershipPlanService>();
        services.AddScoped<IDeleteMembershipPlanService, DeleteMembershipPlanService>();
        services.AddScoped<IGetAllMembershipPlansService, GetAllMembershipPlansService>();
        services.AddScoped<IGetMembershipPlanService, GetMembershipPlanService>();
        services.AddScoped<IUpdateMembershipPlanService, UpdateMembershipPlanService>();

        return services;
    }
}
