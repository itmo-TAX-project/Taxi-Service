using Application.DTO;

namespace Application.Services.Interfaces;

public interface IVehicleService
{
    Task UpdateVehicleAsync(
        VehicleDto vehicle,
        CancellationToken ct);
}