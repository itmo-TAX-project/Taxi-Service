using Application.DTO;
using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Consumer;
using Presentation.Kafka.Consumers.Keys;
using Presentation.Kafka.Consumers.Values;

namespace Presentation.Kafka.Consumers.Handlers;

public class TaxiDriverStatusChangeHandler : IKafkaInboxHandler<TaxiDriverStatusChangeKeyMessage, TaxiDriverStatusChangeMessage>
{
    private readonly IDriverStatusService _driverStatusService;

    public TaxiDriverStatusChangeHandler(IDriverStatusService driverStatusService)
    {
        _driverStatusService = driverStatusService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaInboxMessage<TaxiDriverStatusChangeKeyMessage, TaxiDriverStatusChangeMessage>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaInboxMessage<TaxiDriverStatusChangeKeyMessage, TaxiDriverStatusChangeMessage> message in messages)
        {
            TaxiDriverStatusChangeMessage value = message.Value;
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