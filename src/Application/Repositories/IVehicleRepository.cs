using Application.DTO;

namespace Application.Repositories;

public interface IVehicleRepository
{
    Task<long> AddAsync(
        VehicleDto vehicle,
        CancellationToken cancellationToken);

    Task<IEnumerable<VehicleDto>> GetByDriverAsync(
        long driverId,
        CancellationToken cancellationToken);
}