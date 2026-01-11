using Application.DTO.Enums;

namespace Application.DTO;

public class VehicleDto
{
    public long VehicleId { get; init; }

    public long DriverId { get; init; }

    public VehicleSegment Segment { get; init; }

    public string Plate { get; init; } = string.Empty;

    public string Model { get; init; } = string.Empty;

    public int Capacity { get; init; }
}