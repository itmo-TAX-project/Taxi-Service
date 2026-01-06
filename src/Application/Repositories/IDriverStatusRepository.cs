using Application.DTO;

namespace Application.Repositories;

public interface IDriverStatusRepository
{
    Task AddSnapshotAsync(
        DriverStatusDto snapshots,
        CancellationToken ct);

    Task<DriverStatusDto?> GetLatestAsync(
        Guid driverId,
        CancellationToken ct);
}