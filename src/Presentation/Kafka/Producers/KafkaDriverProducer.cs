using Application.Ports.ProducersPorts;
using Application.Ports.ProducersPorts.Events;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;
using Presentation.Kafka.Producers.Keys;
using Presentation.Kafka.Producers.Values;

namespace Presentation.Kafka.Producers;

public class KafkaDriverProducer : IDriverProducer
{
    private readonly IKafkaMessageProducer<TaxiDriverCreatedMessageKey, TaxiDriverCreatedMessage> _driverCreatedProducer;
    private readonly IKafkaMessageProducer<TaxiDriverStatusChangedMessageKey, TaxiDriverStatusChangedMessage> _statusChangedProducer;
    private readonly IKafkaMessageProducer<TaxiDriverVehicleChangedMessageKey, TaxiDriverVehicleChangedMessage> _vehicleChangedProducer;

    public KafkaDriverProducer(IKafkaMessageProducer<TaxiDriverCreatedMessageKey, TaxiDriverCreatedMessage> driverCreatedProducer, IKafkaMessageProducer<TaxiDriverStatusChangedMessageKey, TaxiDriverStatusChangedMessage> statusChangedProducer, IKafkaMessageProducer<TaxiDriverVehicleChangedMessageKey, TaxiDriverVehicleChangedMessage> vehicleChangedProducer)
    {
        _driverCreatedProducer = driverCreatedProducer;
        _statusChangedProducer = statusChangedProducer;
        _vehicleChangedProducer = vehicleChangedProducer;
    }

    public async Task ProduceAsync(TaxiDriverCreatedEvent driverCreatedEvent, CancellationToken cancellationToken)
    {
        var key = new TaxiDriverCreatedMessageKey { AccountId = driverCreatedEvent.AccountId };
        var message = new TaxiDriverCreatedMessage
        {
            AccountId = driverCreatedEvent.AccountId,
            DriverId = driverCreatedEvent.DriverId,
        };
        var kafkaMessage = new KafkaProducerMessage<TaxiDriverCreatedMessageKey, TaxiDriverCreatedMessage>(key, message);

        await _driverCreatedProducer.ProduceAsync(kafkaMessage, cancellationToken);
    }

    public async Task ProduceAsync(TaxiDriverStatusChangedEvent driverStatusChangedEvent, CancellationToken cancellationToken)
    {
        var key = new TaxiDriverStatusChangedMessageKey { DriverId = driverStatusChangedEvent.DriverId };
        var message = new TaxiDriverStatusChangedMessage
        {
            DriverId = driverStatusChangedEvent.DriverId,
            Latitude = driverStatusChangedEvent.Latitude,
            Longitude = driverStatusChangedEvent.Longitude,
            Availability = driverStatusChangedEvent.Availability,
            Timestamp = driverStatusChangedEvent.Timestamp,
        };
        var kafkaMessage = new KafkaProducerMessage<TaxiDriverStatusChangedMessageKey, TaxiDriverStatusChangedMessage>(key, message);

        await _statusChangedProducer.ProduceAsync(kafkaMessage, cancellationToken);
    }

    public async Task ProduceAsync(TaxiDriverVehicleChangedEvent driverVehicleChangedEvent, CancellationToken cancellationToken)
    {
        var key = new TaxiDriverVehicleChangedMessageKey { DriverId = driverVehicleChangedEvent.DriverId };
        var message = new TaxiDriverVehicleChangedMessage
        {
            DriverId = driverVehicleChangedEvent.DriverId,
            VehicleId = driverVehicleChangedEvent.VehicleId,
            Segment = driverVehicleChangedEvent.Segment,
        };
        var kafkaMessage = new KafkaProducerMessage<TaxiDriverVehicleChangedMessageKey, TaxiDriverVehicleChangedMessage>(key, message);

        await _vehicleChangedProducer.ProduceAsync(kafkaMessage, cancellationToken);
    }
}