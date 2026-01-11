using Application.Repositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IDriverAllowedSegmentsRepository, DriverAllowedSegmentsRepository>()
            .AddScoped<IDriverRepository, DriverRepository>()
            .AddScoped<IDriverStatusRepository, DriverStatusRepository>()
            .AddScoped<IVehicleRepository, VehicleRepository>();
    }
}