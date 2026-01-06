namespace Application.DTO;

public class VehicleDto
{
    public long VehicleId { get; init; }

    public long DriverId { get; init; }

    public string Segment { get; init; } = string.Empty;

    public string Plate { get; init; } = string.Empty;

    public string Model { get; init; } = string.Empty;

    public int Capacity { get; init; }
}