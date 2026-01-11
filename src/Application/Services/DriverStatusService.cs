using Application.DTO;
using Application.DTO.Enums;
using Application.Kafka.Producers.MessageValues;
using Application.Repositories;
using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;
using System.Transactions;

namespace Application.Services;

// TODO: уточнить, что наличие кафки в Application - это норма
public sealed class DriverStatusService(
    IDriverStatusRepository statusRepository,
    IKafkaMessageProducer<long, TaxiDriverStatusChangedMessage> kafkaMessageProducer)
    : IDriverStatusService
{
    public async Task UpdateStatusAsync(DriverStatusDto status, CancellationToken cancellationToken)
    {
        using TransactionScope transaction = CreateTransactionScope();

        await statusRepository.AddSnapshotAsync(status, cancellationToken);

        var message = new TaxiDriverStatusChangedMessage
        {
            DriverId = status.DriverId,
            Status = status.Availability.ToString(),
        };

        var kafkaMessage = new KafkaProducerMessage<long, TaxiDriverStatusChangedMessage>(status.DriverId, message);

        await kafkaMessageProducer.ProduceAsync(kafkaMessage, cancellationToken);
        transaction.Complete();
    }

    public async Task<bool> ValidateDriverActiveAsync(long driverId, CancellationToken cancellationToken)
    {
        DriverStatusDto? latest = await statusRepository.GetLatestAsync(driverId, cancellationToken);

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