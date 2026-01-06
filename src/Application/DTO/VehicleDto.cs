namespace Application.DTO;

public class VehicleDto
{
    public Guid VehicleId { get; init; }

    public Guid DriverId { get; init; }

    public string Segment { get; init; } = string.Empty;

    public string Plate { get; init; } = string.Empty;

    public string Model { get; init; } = string.Empty;

    public int Capacity { get; init; }
}