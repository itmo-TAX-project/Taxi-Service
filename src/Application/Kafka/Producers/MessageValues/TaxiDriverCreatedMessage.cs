namespace Application.Kafka.Producers.MessageValues;

public sealed class TaxiDriverCreatedMessage
{
    public long DriverId { get; set; }

    public long AccountId { get; set; }
}