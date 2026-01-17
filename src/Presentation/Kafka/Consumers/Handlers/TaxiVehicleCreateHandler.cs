using Application.DTO;
using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Consumer;
using Presentation.Kafka.Consumers.Keys;
using Presentation.Kafka.Consumers.Values;

namespace Presentation.Kafka.Consumers.Handlers;

public class TaxiVehicleCreateHandler : IKafkaInboxHandler<TaxiVehicleCreateKeyMessage, TaxiVehicleCreateMessage>
{
    private readonly IVehicleService _vehicleService;

    public TaxiVehicleCreateHandler(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaInboxMessage<TaxiVehicleCreateKeyMessage, TaxiVehicleCreateMessage>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaInboxMessage<TaxiVehicleCreateKeyMessage, TaxiVehicleCreateMessage> message in messages)
        {
            TaxiVehicleCreateMessage value = message.Value;
            var vehicleDto = new VehicleDto
            {
                VehicleId = 0,
                DriverId = value.DriverId,
                Segment = value.Segment,
                Plate = value.Plate,
                Model = value.Model,
                Capacity = value.Capacity,
            };

            await _vehicleService.CreateVehicleAsync(vehicleDto, cancellationToken);
        }
    }
}