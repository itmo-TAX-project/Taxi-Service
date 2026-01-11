using Application.Kafka.Consumers.MessageValues;
using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Consumer;

namespace Application.Kafka.Consumers.Handlers;

public sealed class TaxiVehicleChangeHandler : IKafkaInboxHandler<long, TaxiVehicleChangeMessage>
{
    private readonly IVehicleService _vehicleService;

    public TaxiVehicleChangeHandler(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaInboxMessage<long, TaxiVehicleChangeMessage>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaInboxMessage<long, TaxiVehicleChangeMessage> message in messages)
        {
            await _vehicleService.UpdateVehicleAsync(message.Value.DriverId, message.Value.VehicleId, cancellationToken);
        }
    }
}