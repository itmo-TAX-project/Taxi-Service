using Application.DTO;

namespace Application.Services.Interfaces;

public interface IDriverStatusService
{
    Task UpdateStatusAsync(
        DriverStatusDto status,
        CancellationToken ct);

    Task<bool> ValidateDriverActiveAsync(
        Guid driverId,
        CancellationToken ct);
}