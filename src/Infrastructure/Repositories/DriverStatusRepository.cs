using Application.DTO;
using Application.DTO.Enums;
using Application.Repositories;
using Npgsql;

namespace Infrastructure.Repositories;

public sealed class DriverStatusRepository(NpgsqlDataSource dataSource) : IDriverStatusRepository
{
    public async Task AddSnapshotAsync(DriverStatusDto snapshot, CancellationToken cancellationToken)
    {
        const string sql = """
        insert into driver_status_snapshots (driver_id, latitude, longitude, availability, ts)
        values (@driver_id, @lat, @lon, @availability, @timestamp);
        """;

        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("driver_id", snapshot.DriverId);
        command.Parameters.AddWithValue("lat", snapshot.Latitude);
        command.Parameters.AddWithValue("lon", snapshot.Longitude);
        command.Parameters.AddWithValue("availability", snapshot.Availability);
        command.Parameters.AddWithValue("timestamp", snapshot.Timestamp);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<DriverStatusDto?> GetLatestAsync(long driverId, CancellationToken cancellationToken)
    {
        const string sql = """
        select driver_id, latitude, longitude, availability, ts
        from driver_status_snapshots
        where driver_id = @driver_id
        order by ts desc
        limit 1;
        """;

        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("driver_id", driverId);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new DriverStatusDto
        {
            DriverId = reader.GetInt64(0),
            Latitude = reader.GetDouble(1),
            Longitude = reader.GetDouble(2),
            Availability = reader.GetFieldValue<DriverAvailability>(3),
            Timestamp = reader.GetDateTime(4),
        };
    }
}
