using Application.DTO;

namespace Application.Repositories;

public interface IVehicleRepository
{
    Task AddAsync(
        IEnumerable<VehicleDto> vehicles,
        CancellationToken ct);

    Task<IEnumerable<VehicleDto>> GetByDriverAsync(
        Guid driverId,
        CancellationToken ct);
}