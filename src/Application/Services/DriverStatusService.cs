using Application.DTO;
using Application.DTO.Enums;
using Application.Repositories;
using Application.Services.Interfaces;
using System.Text.Json;
using System.Transactions;

namespace Application.Services;

public sealed class DriverStatusService(
    IDriverStatusRepository statusRepository,
    IOutboxRepository outboxRepository)
    : IDriverStatusService
{
    public async Task UpdateStatusAsync(
        DriverStatusDto status,
        CancellationToken ct)
    {
        using TransactionScope transaction = CreateTransactionScope();

        await statusRepository.AddSnapshotAsync(status, ct);

        var evt = new OutboxEventDto
        {
            EventType = "taxi.driver.status_changed",
            OccurredAt = DateTime.UtcNow,
            Payload = JsonSerializer.Serialize(status),
        };

        await outboxRepository.AddAsync(evt, ct);

        transaction.Complete();
    }

    public async Task<bool> ValidateDriverActiveAsync(
        long driverId,
        CancellationToken ct)
    {
        DriverStatusDto? latest = await statusRepository.GetLatestAsync(driverId, ct);

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