using Application.DTO.Enums;

namespace Presentation.Kafka.Producers.Values;

public class TaxiDriverVehicleChangedMessage
{
    public long DriverId { get; set; }

    public long VehicleId { get; set; }

    public VehicleSegment Segment { get; set; }
}