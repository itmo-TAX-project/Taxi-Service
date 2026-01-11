using Application.DTO.Enums;

namespace Application.Repositories;

public interface IDriverAllowedSegmentsRepository
{
    Task AddDriverSegmentsAsync(
        (long DriverId, VehicleSegment Segment) allowedSegments,
        CancellationToken cancellationToken);

    Task RemoveDriverSegmentsAsync(
        long driverId,
        CancellationToken cancellationToken);

    Task RemoveDriverSegmentAsync(
        (long DriverId, VehicleSegment Segment) segments,
        CancellationToken cancellationToken);

    Task<IEnumerable<VehicleSegment>> GetDriverAllowedSegmentsAsync(
        long driverId,
        CancellationToken cancellationToken);
}