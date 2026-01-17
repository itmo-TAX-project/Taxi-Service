using Application.DTO;
using Application.DTO.Enums;
using Application.Ports.ProducersPorts;
using Application.Ports.ProducersPorts.Events;
using Application.Repositories;
using Application.Services.Interfaces;
using System.Transactions;

namespace Application.Services;

public sealed class DriverStatusService : IDriverStatusService
{
    private readonly IDriverStatusRepository _statusRepository;
    private readonly IDriverProducer _driverProducer;

    public DriverStatusService(IDriverStatusRepository statusRepository, IDriverProducer driverProducer)
    {
        _statusRepository = statusRepository;
        _driverProducer = driverProducer;
    }

    public async Task UpdateStatusAsync(DriverStatusDto status, CancellationToken cancellationToken)
    {
        using TransactionScope transaction = CreateTransactionScope();

        await _statusRepository.AddSnapshotAsync(status, cancellationToken);

        var message = new TaxiDriverStatusChangedEvent
        {
            DriverId = status.DriverId,
            Availability = status.Availability,
            Longitude = status.Longitude,
            Latitude = status.Latitude,
            Timestamp = status.Timestamp,
        };
        await _driverProducer.ProduceAsync(message, cancellationToken);
        transaction.Complete();
    }

    public async Task<bool> ValidateDriverActiveAsync(long driverId, CancellationToken cancellationToken)
    {
        DriverStatusDto? latest = await _statusRepository.GetLatestAsync(driverId, cancellationToken);

        return latest?.Availability is DriverAvailability.Searching;
    }

    private static TransactionScope CreateTransactionScope()
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}