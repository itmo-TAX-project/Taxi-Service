using Application.DTO;

namespace Application.Services.Interfaces;

public interface IVehicleService
{
    Task<long> CreateVehicleAsync(VehicleDto vehicle, CancellationToken cancellationToken);

    Task UpdateVehicleAsync(long driverId, long vehicleId, CancellationToken cancellationToken);
}