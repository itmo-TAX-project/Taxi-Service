namespace Presentation.Kafka.Producers.Values;

public sealed class TaxiDriverCreatedMessage
{
    public long DriverId { get; set; }

    public long AccountId { get; set; }
}