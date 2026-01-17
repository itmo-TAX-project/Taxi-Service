using Application.DTO.Enums;

namespace Presentation.Kafka.Consumers.Values;

public class TaxiDriverCreateMessage
{
    public long AccountId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string LicenseNumber { get; set; } = string.Empty;

    public long? CurrentVehicleId { get; set; }

    public decimal Rating { get; set; }

    public IEnumerable<VehicleSegment> AllowedSegments { get; set; } = [];
}