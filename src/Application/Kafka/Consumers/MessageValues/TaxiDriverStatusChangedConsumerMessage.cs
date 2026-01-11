using Application.DTO.Enums;

namespace Application.Kafka.Consumers.MessageValues;

public class TaxiDriverStatusChangedConsumerMessage
{
    public long DriverId { get; init; }

    public double Latitude { get; init; }

    public double Longitude { get; init; }

    public DriverAvailability Availability { get; init; }

    public DateTime Timestamp { get; init; }
}