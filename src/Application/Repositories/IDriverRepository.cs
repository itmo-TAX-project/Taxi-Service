using Application.DTO;

namespace Application.Repositories;

public interface IDriverRepository
{
    Task CreateAsync(
        DriverDto drivers,
        CancellationToken ct);

    Task<DriverDto?> GetByAccountIdAsync(
        long accountId,
        CancellationToken ct);

    Task SetCurrentVehicleAsync(
        (long DriverId, long VehicleId) updates,
        CancellationToken ct);

    Task DeleteDriverAsync(
        long driverId,
        CancellationToken ct);
}