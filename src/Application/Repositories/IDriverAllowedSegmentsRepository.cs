using Application.DTO.Enums;

namespace Application.Repositories;

public interface IDriverAllowedSegmentsRepository
{
    Task AddDriverSegmentsAsync(
        (long DriverId, VehicleSegment Segment) allowedSegments,
        CancellationToken ct);

    Task RemoveDriverSegmentsAsync(long driverId, CancellationToken ct);

    Task RemoveDriverSegmentAsync((long DriverId, VehicleSegment Segment) segments, CancellationToken ct);

    Task<IEnumerable<VehicleSegment>> GetDriverAllowedSegmentsAsync(
        long driverId,
        CancellationToken ct);
}