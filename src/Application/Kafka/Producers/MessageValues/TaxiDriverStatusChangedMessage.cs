namespace Application.Kafka.Producers.MessageValues;

public class TaxiDriverStatusChangedMessage
{
    public long DriverId { get; init; }

    public string Status { get; set; } = string.Empty;
}