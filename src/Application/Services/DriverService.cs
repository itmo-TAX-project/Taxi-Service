using Application.DTO;
using Application.DTO.Enums;
using Application.Kafka.Producers.MessageValues;
using Application.Repositories;
using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;
using System.Transactions;

namespace Application.Services;

public sealed class DriverService(
    IDriverRepository driverRepository,
    IDriverAllowedSegmentsRepository segmentsRepository,
    IKafkaMessageProducer<long, TaxiDriverCreatedMessage> kafkaMessageProducer)
    : IDriverService
{
    public async Task CreateDriverAsync(
        DriverDto driver,
        IEnumerable<VehicleSegment> allowedSegments,
        CancellationToken cancellationToken)
    {
        using TransactionScope transaction = CreateTransactionScope();

        long driverId = await driverRepository.CreateAsync(driver, cancellationToken);

        foreach (VehicleSegment segment in allowedSegments)
        {
            await segmentsRepository.AddDriverSegmentsAsync((driverId, segment), cancellationToken);
        }

        var message = new TaxiDriverCreatedMessage
        {
            DriverId = driverId,
            AccountId = driver.AccountId,
        };

        var kafkaMessage = new KafkaProducerMessage<long, TaxiDriverCreatedMessage>(driver.AccountId, message);

        await kafkaMessageProducer.ProduceAsync(kafkaMessage, cancellationToken);

        transaction.Complete();
    }

    public Task<DriverDto?> GetDriverAsync(long accountId, CancellationToken cancellationToken)
    {
        return driverRepository.GetByAccountIdAsync(accountId, cancellationToken);
    }

    public async Task<IEnumerable<VehicleSegment>> GetAllowedSegmentsAsyncByAccountId(
        long accountId,
        CancellationToken cancellationToken)
    {
        using TransactionScope transaction = CreateTransactionScope();

        DriverDto? driver = await driverRepository.GetByAccountIdAsync(accountId, cancellationToken);

        if (driver == null)
            throw new NullReferenceException("Driver not found");

        IEnumerable<VehicleSegment> segments = await segmentsRepository
            .GetDriverAllowedSegmentsAsync(driver.DriverId, cancellationToken);

        transaction.Complete();

        return segments;
    }

    public async Task<IEnumerable<VehicleSegment>> GetAllowedSegmentsAsyncByDriverId(
        long driverId,
        CancellationToken cancellationToken)
    {
        using TransactionScope transaction = CreateTransactionScope();

        IEnumerable<VehicleSegment> segments = await segmentsRepository
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