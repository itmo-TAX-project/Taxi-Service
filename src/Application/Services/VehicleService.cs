using Application.DTO;
using Application.Ports.ProducersPorts;
using Application.Ports.ProducersPorts.Events;
using Application.Repositories;
using Application.Services.Interfaces;
using System.Transactions;

namespace Application.Services;

public sealed class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IDriverRepository _driverRepository;
    private readonly IDriverProducer _driverProducer;

    public VehicleService(
        IVehicleRepository vehicleRepository,
        IDriverRepository driverRepository,
        IDriverProducer driverProducer)
    {
        _vehicleRepository = vehicleRepository;
        _driverRepository = driverRepository;
        _driverProducer = driverProducer;
    }

    public async Task<long> CreateVehicleAsync(VehicleDto vehicle, CancellationToken cancellationToken)
    {
        using TransactionScope transaction = CreateTransactionScope();

        long vehicleId = await _vehicleRepository.AddAsync(vehicle, cancellationToken);

        transaction.Complete();

        return vehicleId;
    }

    public async Task UpdateVehicleAsync(long driverId, long vehicleId, CancellationToken cancellationToken)
    {
        using TransactionScope transaction = CreateTransactionScope();

        await _driverRepository.SetCurrentVehicleAsync((driverId, vehicleId), cancellationToken);

        IEnumerable<VehicleDto> vehicles = await _vehicleRepository.GetByDriverAsync(driverId, cancellationToken);

        VehicleDto vehicleDto = vehicles.First(a => a.VehicleId == vehicleId);

        var message = new TaxiDriverVehicleChangedEvent
        {
            DriverId = vehicleDto.DriverId,
            VehicleId = vehicleDto.VehicleId,
            Segment = vehicleDto.Segment,
        };
        await _driverProducer.ProduceAsync(message, cancellationToken);

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