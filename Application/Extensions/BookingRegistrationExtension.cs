using Application.Abstraction.BookingsInterface;
using Application.Bookings.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class BookingRegistrationExtension
{
    public static IServiceCollection AddBookingServices(this IServiceCollection services)
    {
        services.AddScoped<ICreateBookingService, CreateBookingService>();
        services.AddScoped<IDeleteBookingService, DeleteBookingService>();

        return services;
    }
}
