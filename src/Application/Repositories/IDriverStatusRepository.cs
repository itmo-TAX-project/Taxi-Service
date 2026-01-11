using Application.DTO;

namespace Application.Repositories;

public interface IDriverStatusRepository
{
    Task AddSnapshotAsync(
        DriverStatusDto snapshot,
        CancellationToken cancellationToken);

    Task<DriverStatusDto?> GetLatestAsync(
        long driverId,
        CancellationToken cancellationToken);
}