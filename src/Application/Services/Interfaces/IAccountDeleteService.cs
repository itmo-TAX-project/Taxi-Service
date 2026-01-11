namespace Application.Services.Interfaces;

public interface IAccountDeleteService
{
    Task AccountDeleteAsync(long accountId, CancellationToken cancellationToken);
}