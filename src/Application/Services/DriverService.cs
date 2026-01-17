using Application.DTO;
using Application.DTO.Enums;
using Application.Ports.ProducersPorts;
using Application.Ports.ProducersPorts.Events;
using Application.Repositories;
using Application.Services.Interfaces;
using System.Transactions;

namespace Application.Services;

public sealed class DriverService : IDriverService
{
    private readonly IDriverRepository _driverRepository;
    private readonly IDriverAllowedSegmentsRepository _segmentsRepository;
    private readonly IDriverProducer _driverProducer;

    public DriverService(
        IDriverRepository driverRepository,
        IDriverAllowedSegmentsRepository segmentsRepository,
        IDriverProducer driverProducer)
    {
        _driverRepository = driverRepository;
        _segmentsRepository = segmentsRepository;
        _driverProducer = driverProducer;
    }

    public async Task CreateDriverAsync(
        DriverDto driver,
        IEnumerable<VehicleSegment> allowedSegments,
        CancellationToken cancellationToken)
    {
        using TransactionScope transaction = CreateTransactionScope();

        long driverId = await _driverRepository.CreateAsync(driver, cancellationToken);

        foreach (VehicleSegment segment in allowedSegments)
        {
            await _segmentsRepository.AddDriverSegmentsAsync((driverId, segment), cancellationToken);
        }

        var message = new TaxiDriverCreatedEvent
        {
            DriverId = driverId,
            AccountId = driver.AccountId,
        };
        await _driverProducer.ProduceAsync(message, cancellationToken);

        transaction.Complete();
    }

    public Task<DriverDto?> GetDriverAsync(long accountId, CancellationToken cancellationToken)
    {
        return _driverRepository.GetByAccountIdAsync(accountId, cancellationToken);
    }

    public async Task<IEnumerable<VehicleSegment>> GetAllowedSegmentsAsyncByAccountId(
        long accountId,
        CancellationToken cancellationToken)
    {
        using TransactionScope transaction = CreateTransactionScope();

        DriverDto? driver = await _driverRepository.GetByAccountIdAsync(accountId, cancellationToken);

        if (driver == null)
            throw new NullReferenceException("Driver not found");

        IEnumerable<VehicleSegment> segments = await _segmentsRepository
            .GetDriverAllowedSegmentsAsync(driver.DriverId, cancellationToken);

        transaction.Complete();

        return segments;
    }

    public async Task<IEnumerable<VehicleSegment>> GetAllowedSegmentsAsyncByDriverId(
        long driverId,
        CancellationToken cancellationToken)
    {
        using TransactionScope transaction = CreateTransactionScope();

        IEnumerable<VehicleSegment> segments = await _segmentsRepository
            .GetDriverAllowedSegmentsAsync(driverId, cancellationToken);

        transaction.Complete();

        return segments;
    }

    private static TransactionScope CreateTransactionScope()
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}