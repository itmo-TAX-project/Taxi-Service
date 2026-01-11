using Application.DTO;

namespace Application.Repositories;

public interface IDriverRepository
{
    Task<long> CreateAsync(
        DriverDto drivers,
        CancellationToken cancellationToken);

    Task<DriverDto?> GetByAccountIdAsync(
        long accountId,
        CancellationToken cancellationToken);

    Task SetCurrentVehicleAsync(
        (long DriverId, long VehicleId) updates,
        CancellationToken cancellationToken);

    Task DeleteDriverAsync(
        long driverId,
        CancellationToken cancellationToken);
}