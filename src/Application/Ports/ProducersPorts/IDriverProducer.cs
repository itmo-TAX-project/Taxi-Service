using Application.Ports.ProducersPorts.Events;

namespace Application.Ports.ProducersPorts;

public interface IDriverProducer
{
    Task ProduceAsync(TaxiDriverCreatedEvent driverCreatedEvent, CancellationToken cancellationToken);

    Task ProduceAsync(TaxiDriverStatusChangedEvent driverStatusChangedEvent, CancellationToken cancellationToken);

    Task ProduceAsync(TaxiDriverVehicleChangedEvent driverVehicleChangedEvent, CancellationToken cancellationToken);
}