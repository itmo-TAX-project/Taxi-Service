using Application.DTO.Enums;

namespace Application.Repositories;

public interface IDriverAllowedSegmentsRepository
{
    Task AddDriverSegmentsAsync(
        (Guid DriverId, VehicleSegment Segment) allowedSegments,
        CancellationToken ct);

    Task RemoveDriverSegmentsAsync(Guid driverId, CancellationToken ct);

    Task RemoveDriverSegmentAsync((Guid DriverId, VehicleSegment Segment) segments, CancellationToken ct);

    Task<IEnumerable<VehicleSegment>> GetDriverAllowedSegmentsAsync(
        Guid driverId,
        CancellationToken ct);
}