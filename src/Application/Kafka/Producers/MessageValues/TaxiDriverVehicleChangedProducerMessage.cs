using Application.DTO.Enums;

namespace Application.Kafka.Producers.MessageValues;

public class TaxiDriverVehicleChangedProducerMessage
{
    public long DriverId { get; set; }

    public long VehicleId { get; set; }

    public VehicleSegment Segment { get; set; }
}