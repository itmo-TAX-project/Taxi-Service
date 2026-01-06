using Application.DTO;
using Application.Repositories;
using Application.Services.Interfaces;
using System.Transactions;

namespace Application.Services;

public class AccountDeleteService(
    IDriverRepository driverRepository) : IAccountDeleteService
{
    public async Task HandleAccountDeletedAsync(
        long accountId,
        CancellationToken ct)
    {
        using TransactionScope transaction = CreateTransactionScope();

        DriverDto? driver = await driverRepository.GetByAccountIdAsync(accountId, ct);
        if (driver is null)
            return;

        await driverRepository.DeleteDriverAsync(driver.DriverId, ct);

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