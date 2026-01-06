using Application.DTO;
using Application.DTO.Enums;
using Application.Repositories;
using Application.Services.Interfaces;
using System.Text.Json;
using System.Transactions;

namespace Application.Services;

public sealed class DriverService(
    IDriverRepository driverRepository,
    IDriverAllowedSegmentsRepository segmentsRepository,
    IOutboxRepository outboxRepository)
    : IDriverService
{
    public async Task CreateDriverAsync(
        DriverDto driver,
        IEnumerable<VehicleSegment> allowedSegments,
        CancellationToken ct)
    {
        using TransactionScope transaction = CreateTransactionScope();

        await driverRepository.CreateAsync(driver, ct);

        foreach (VehicleSegment segment in allowedSegments)
        {
            await segmentsRepository.AddDriverSegmentsAsync(
                (driver.DriverId, segment),
                ct);
        }

        var evt = new OutboxEventDto
        {
            EventType = "taxi.driver.created",
            OccurredAt = DateTime.UtcNow,
            Payload = JsonSerializer.Serialize(new
            {
                driver.DriverId,
                driver.AccountId,
            }),
        };

        await outboxRepository.AddAsync(evt, ct);

        transaction.Complete();
    }

    public Task<DriverDto?> GetDriverAsync(
        long accountId,
        CancellationToken ct)
    {
        return driverRepository.GetByAccountIdAsync(accountId, ct);
    }

    private static TransactionScope CreateTransactionScope()
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}