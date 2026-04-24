using Application.Abstraction.TrainingSessionsInterface;
using Application.TrainingSessions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class TrainingSessionRegistrationExtension
{
    public static IServiceCollection AddTrainingSessionServices(this IServiceCollection services)
    {
        services.AddScoped<ICreateTrainingSessionService, CreateTrainingSessionService>();
        services.AddScoped<IUpdateTrainingSessionService, UpdateTrainingSessionService>();
        services.AddScoped<IGetTrainingSessionService, GetTrainingSessionService>();
        services.AddScoped<IGetAllTrainingSessionsService, GetAllTrainingSessionsService>();
        services.AddScoped<IDeleteTrainingSessionService, DeleteTrainingSessionService>();

        return services;
    }
}
