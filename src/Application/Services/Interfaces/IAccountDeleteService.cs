namespace Application.Services.Interfaces;

public interface IAccountDeleteService
{
    Task HandleAccountDeletedAsync(
        long accountId,
        CancellationToken ct);
}