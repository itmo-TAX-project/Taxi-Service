using Application.DTO;
using Application.Kafka.Producers.MessageValues;
using Application.Repositories;
using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;
using System.Transactions;

namespace Application.Services;

public sealed class VehicleService(
    IVehicleRepository vehicleRepository,
    IDriverRepository driverRepository,
    IKafkaMessageProducer<long, TaxiDriverVehicleChangedProducerMessage> kafkaMessageProducer)
    : IVehicleService
{
    public async Task<long> CreateVehicleAsync(VehicleDto vehicle, CancellationToken cancellationToken)
    {
        using TransactionScope transaction = CreateTransactionScope();

        long vehicleId = await vehicleRepository.AddAsync(vehicle, cancellationToken);

        transaction.Complete();

        return vehicleId;
    }

    public async Task UpdateVehicleAsync(long driverId, long vehicleId, CancellationToken cancellationToken)
    {
        using TransactionScope transaction = CreateTransactionScope();

        await driverRepository.SetCurrentVehicleAsync((driverId, vehicleId), cancellationToken);

        IEnumerable<VehicleDto> vehicles = await vehicleRepository.GetByDriverAsync(driverId, cancellationToken);

        VehicleDto vehicleDto = vehicles.First(a => a.VehicleId == vehicleId);

        var message = new TaxiDriverVehicleChangedProducerMessage
        {
            DriverId = vehicleDto.DriverId,
            VehicleId = vehicleDto.VehicleId,
            Segment = vehicleDto.Segment,
        };

        var kafkaMessage = new KafkaProducerMessage<long, TaxiDriverVehicleChangedProducerMessage>(
            vehicleDto.DriverId,
            message);

        await kafkaMessageProducer.ProduceAsync(kafkaMessage, cancellationToken);

        transaction.Complete();
    }

    private static TransactionScope CreateTransactionScope()
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}