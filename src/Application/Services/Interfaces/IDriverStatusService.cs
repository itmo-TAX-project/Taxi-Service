using Application.DTO;

namespace Application.Services.Interfaces;

public interface IDriverStatusService
{
    Task UpdateStatusAsync(
        DriverStatusDto status,
        CancellationToken ct);

    Task<bool> ValidateDriverActiveAsync(
        long driverId,
        CancellationToken ct);
}