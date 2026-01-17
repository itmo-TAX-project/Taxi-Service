using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Consumer;
using Presentation.Kafka.Consumers.Keys;
using Presentation.Kafka.Consumers.Values;

namespace Presentation.Kafka.Consumers.Handlers;

public sealed class TaxiVehicleChangeHandler : IKafkaInboxHandler<TaxiVehicleChangeKeyMessage, TaxiVehicleChangeMessage>
{
    private readonly IVehicleService _vehicleService;

    public TaxiVehicleChangeHandler(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaInboxMessage<TaxiVehicleChangeKeyMessage, TaxiVehicleChangeMessage>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaInboxMessage<TaxiVehicleChangeKeyMessage, TaxiVehicleChangeMessage> message in messages)
        {
            await _vehicleService.UpdateVehicleAsync(message.Value.DriverId, message.Value.VehicleId, cancellationToken);
        }
    }
}