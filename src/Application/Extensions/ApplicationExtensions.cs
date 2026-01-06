using Application.Services;
using Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDriverService, DriverService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IDriverStatusService, DriverStatusService>();
        services.AddScoped<IAccountDeleteService, AccountDeleteService>();

        return services;
    }
}