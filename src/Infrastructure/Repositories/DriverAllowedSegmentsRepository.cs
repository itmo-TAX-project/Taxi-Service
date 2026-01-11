using Application.DTO.Enums;
using Application.Repositories;
using Npgsql;

namespace Infrastructure.Repositories;

public sealed class DriverAllowedSegmentsRepository(NpgsqlDataSource dataSource) : IDriverAllowedSegmentsRepository
{
    public async Task AddDriverSegmentsAsync(
        (long DriverId, VehicleSegment Segment) allowedSegments,
        CancellationToken cancellationToken)
    {
        const string sql = """
        insert into driver_allowed_segments (driver_id, segment)
        values (@driver_id, @segment)
        on conflict do nothing;
        """;

        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("driver_id", allowedSegments.DriverId);
        command.Parameters.AddWithValue("segment", allowedSegments.Segment);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task RemoveDriverSegmentsAsync(long driverId, CancellationToken cancellationToken)
    {
        const string sql = "delete from driver_allowed_segments where driver_id = @driver_id;";

        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("driver_id", driverId);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task RemoveDriverSegmentAsync(
        (long DriverId, VehicleSegment Segment) segments,
        CancellationToken cancellationToken)
    {
        const string sql = """
        delete from driver_allowed_segments
        where driver_id = @driver_id and segment = @segment;
        """;

        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("driver_id", segments.DriverId);
        command.Parameters.AddWithValue("segment", segments.Segment.ToString().ToLowerInvariant());

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IEnumerable<VehicleSegment>> GetDriverAllowedSegmentsAsync(
        long driverId,
        CancellationToken cancellationToken)
    {
        const string sql = """
        select segment
        from driver_allowed_segments
        where driver_id = @driver_id;
        """;

        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("driver_id", driverId);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        var result = new List<VehicleSegment>();
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(Enum.Parse<VehicleSegment>(reader.GetString(0), true));
        }

        return result;
    }
}
