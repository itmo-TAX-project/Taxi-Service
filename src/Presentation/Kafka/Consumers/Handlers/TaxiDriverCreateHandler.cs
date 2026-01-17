using Application.DTO;
using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Consumer;
using Presentation.Kafka.Consumers.Keys;
using Presentation.Kafka.Consumers.Values;

namespace Presentation.Kafka.Consumers.Handlers;

public sealed class TaxiDriverCreateHandler : IKafkaInboxHandler<TaxiDriverCreateKeyMessage, TaxiDriverCreateMessage>
{
    private readonly IDriverService _driverService;

    public TaxiDriverCreateHandler(IDriverService driverService)
    {
        _driverService = driverService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaInboxMessage<TaxiDriverCreateKeyMessage, TaxiDriverCreateMessage>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaInboxMessage<TaxiDriverCreateKeyMessage, TaxiDriverCreateMessage> message in messages)
        {
            TaxiDriverCreateMessage value = message.Value;
            var driverDto = new DriverDto
            {
                DriverId = 0,
                AccountId = value.AccountId,
                Name = value.Name,
                LicenseNumber = value.LicenseNumber,
                CurrentVehicleId = value.CurrentVehicleId,
                Rating = value.Rating,
            };
            await _driverService.CreateDriverAsync(driverDto, value.AllowedSegments, cancellationToken);
        }
    }
}