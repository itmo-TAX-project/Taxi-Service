using Application.DTO;

namespace Application.Repositories;

public interface IDriverRepository
{
    Task CreateAsync(
        DriverDto drivers,
        CancellationToken ct);

    Task<DriverDto?> GetByAccountIdAsync(
        Guid accountId,
        CancellationToken ct);

    Task SetCurrentVehicleAsync(
        (Guid DriverId, Guid VehicleId) updates,
        CancellationToken ct);

    Task DeleteDriverAsync(
        Guid driverId,
        CancellationToken ct);
}