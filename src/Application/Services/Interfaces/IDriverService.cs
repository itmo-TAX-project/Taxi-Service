using Application.DTO;
using Application.DTO.Enums;

namespace Application.Services.Interfaces;

public interface IDriverService
{
    Task CreateDriverAsync(
        DriverDto driver,
        IEnumerable<VehicleSegment> allowedSegments,
        CancellationToken ct);

    Task<DriverDto?> GetDriverAsync(
        Guid accountId,
        CancellationToken ct);
}