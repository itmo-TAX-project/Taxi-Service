using Application.DTO.Enums;

namespace Application.DTO;

public class AllowedSegmentDto
{
    public long DriverId { get; init; }

    public VehicleSegment Segment { get; init; }
}