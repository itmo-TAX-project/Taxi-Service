using Application.DTO;
using Application.DTO.Enums;

namespace Application.Services.Interfaces;

public interface IDriverService
{
    Task CreateDriverAsync(DriverDto driver, IEnumerable<VehicleSegment> allowedSegments, CancellationToken cancellationToken);

    Task<DriverDto?> GetDriverAsync(long accountId, CancellationToken cancellationToken);

    Task<IEnumerable<VehicleSegment>> GetAllowedSegmentsAsyncByAccountId(
        long accountId,
        CancellationToken cancellationToken);

    Task<IEnumerable<VehicleSegment>> GetAllowedSegmentsAsyncByDriverId(
        long driverId,
        CancellationToken cancellationToken);
}