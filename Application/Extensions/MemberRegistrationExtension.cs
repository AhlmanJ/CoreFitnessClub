
// In this file I register all Services related to Member.

using Application.Abstraction.MembersInterface;
using Application.Members.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class MemberRegistrationExtension
{
    public static IServiceCollection AddMemberServices(this IServiceCollection services)
    {
        services.AddScoped<IRegisterMemberAccountService, RegisterMemberAccountService>();
        services.AddScoped<ISignInMemberService, SignInMemberService>();
        services.AddScoped<IGetMemberProfileService, GetMemberProfileService>();
        services.AddScoped<IUpdateMemberProfileService, UpdateMemberProfileService>();

        return services;
    }
}
