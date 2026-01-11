using Application.DTO;
using Application.Repositories;
using Application.Services.Interfaces;
using System.Transactions;

namespace Application.Services;

public class AccountDeleteService(IDriverRepository driverRepository) : IAccountDeleteService
{
    public async Task AccountDeleteAsync(long accountId, CancellationToken cancellationToken)
    {
        using TransactionScope transaction = CreateTransactionScope();

        DriverDto? driver = await driverRepository.GetByAccountIdAsync(accountId, cancellationToken);
        if (driver is null)
            return;

        await driverRepository.DeleteDriverAsync(driver.DriverId, cancellationToken);

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