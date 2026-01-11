using Application.DTO;
using Application.Kafka.Consumers.MessageValues;
using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Consumer;

namespace Application.Kafka.Consumers.Handlers;

public sealed class TaxiDriverCreateHandler : IKafkaInboxHandler<long, TaxiDriverCreateMessage>
{
    private readonly IDriverService _driverService;

    public TaxiDriverCreateHandler(IDriverService driverService)
    {
        _driverService = driverService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaInboxMessage<long, TaxiDriverCreateMessage>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaInboxMessage<long, TaxiDriverCreateMessage> message in messages)
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