namespace Application.Kafka.Consumers.MessageValues;

public class TaxiVehicleChangeMessage
{
    public long DriverId { get; set; }

    public long VehicleId { get; set; }
}