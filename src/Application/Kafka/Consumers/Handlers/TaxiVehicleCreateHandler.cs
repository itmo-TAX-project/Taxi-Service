using Application.DTO;
using Application.Kafka.Consumers.MessageValues;
using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Consumer;

namespace Application.Kafka.Consumers.Handlers;

public class TaxiVehicleCreateHandler : IKafkaInboxHandler<long, TaxiVehicleCreateMessage>
{
    private readonly IVehicleService _vehicleService;

    public TaxiVehicleCreateHandler(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaInboxMessage<long, TaxiVehicleCreateMessage>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaInboxMessage<long, TaxiVehicleCreateMessage> message in messages)
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