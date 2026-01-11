using Application.DTO;
using Application.Kafka.Consumers.MessageValues;
using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Consumer;

namespace Application.Kafka.Consumers.Handlers;

public class TaxiDriverStatusChangeHandler : IKafkaInboxHandler<long, TaxiDriverStatusChangedConsumerMessage>
{
    private readonly IDriverStatusService _driverStatusService;

    public TaxiDriverStatusChangeHandler(IDriverStatusService driverStatusService)
    {
        _driverStatusService = driverStatusService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaInboxMessage<long, TaxiDriverStatusChangedConsumerMessage>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaInboxMessage<long, TaxiDriverStatusChangedConsumerMessage> message in messages)
        {
            TaxiDriverStatusChangedConsumerMessage value = message.Value;
            var status = new DriverStatusDto
            {
                DriverId = value.DriverId,
                Latitude = value.Latitude,
                Longitude = value.Longitude,
                Availability = value.Availability,
                Timestamp = value.Timestamp,
            };
            await _driverStatusService.UpdateStatusAsync(status, cancellationToken);
        }
    }
}