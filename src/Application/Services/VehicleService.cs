using Application.DTO;
using Application.Repositories;
using Application.Services.Interfaces;
using System.Text.Json;
using System.Transactions;

namespace Application.Services;

public sealed class VehicleService(
    IVehicleRepository vehicleRepository,
    IDriverRepository driverRepository,
    IOutboxRepository outboxRepository)
    : IVehicleService
{
    public async Task UpdateVehicleAsync(
        VehicleDto vehicle,
        CancellationToken ct)
    {
        using TransactionScope transaction = CreateTransactionScope();

        await vehicleRepository.AddAsync([vehicle], ct);

        await driverRepository.SetCurrentVehicleAsync(
            (vehicle.DriverId, vehicle.VehicleId),
            ct);

        var evt = new OutboxEventDto
        {
            EventId = Guid.NewGuid(),
            EventType = "taxi.vehicle.changed",
            OccurredAt = DateTime.UtcNow,
            Payload = JsonSerializer.Serialize(vehicle),
        };

        await outboxRepository.AddAsync(evt, ct);

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
