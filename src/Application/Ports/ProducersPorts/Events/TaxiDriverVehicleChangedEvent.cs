using Application.DTO.Enums;

namespace Application.Ports.ProducersPorts.Events;

public class TaxiDriverVehicleChangedEvent
{
    public long DriverId { get; set; }

    public long VehicleId { get; set; }

    public VehicleSegment Segment { get; set; }
}