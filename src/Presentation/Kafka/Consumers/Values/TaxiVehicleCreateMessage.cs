using Application.DTO.Enums;

namespace Presentation.Kafka.Consumers.Values;

public class TaxiVehicleCreateMessage
{
    public long DriverId { get; init; }

    public VehicleSegment Segment { get; init; }

    public string Plate { get; init; } = string.Empty;

    public string Model { get; init; } = string.Empty;

    public int Capacity { get; init; }
}