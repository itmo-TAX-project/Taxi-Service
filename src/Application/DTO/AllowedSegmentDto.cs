using Application.DTO.Enums;

namespace Application.DTO;

public class AllowedSegmentDto
{
    public Guid DriverId { get; init; }

    public VehicleSegment Segment { get; init; }
}