namespace Application.Ports.ProducersPorts.Events;

public class TaxiDriverCreatedEvent : IEventMessage
{
    public long DriverId { get; set; }

    public long AccountId { get; set; }
}