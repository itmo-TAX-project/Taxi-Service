namespace Application.Services.Interfaces;

public interface IAccountDeleteService
{
    Task HandleAccountDeletedAsync(
        Guid accountId,
        CancellationToken ct);
}