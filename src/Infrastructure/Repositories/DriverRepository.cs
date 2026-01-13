using Application.DTO;
using Application.Repositories;
using Npgsql;

namespace Infrastructure.Repositories;

public sealed class DriverRepository(NpgsqlDataSource dataSource) : IDriverRepository
{
    public async Task<long> CreateAsync(
        DriverDto drivers,
        CancellationToken cancellationToken)
    {
        const string sql = """
        insert into drivers (account_id, name, license_number, rating)
        values (@account_id, @name, @license, @rating)
        returning driver_id;
        """;

        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("account_id", drivers.AccountId);
        command.Parameters.AddWithValue("name", drivers.Name);
        command.Parameters.AddWithValue("license", drivers.LicenseNumber);
        command.Parameters.AddWithValue("rating", drivers.Rating);

        return (long)(await command.ExecuteScalarAsync(cancellationToken) ?? throw new NullReferenceException("Could not create driver"));
    }

    public async Task<DriverDto?> GetByAccountIdAsync(
        long accountId,
        CancellationToken cancellationToken)
    {
        const string sql = """
                           select driver_id, account_id, name, license_number, current_vehicle_id, rating
                           from drivers
                           where account_id = @account_id;
                           """;

        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("account_id", accountId);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new DriverDto
        {
            DriverId = reader.GetInt64(0),
            AccountId = reader.GetInt64(1),
            Name = reader.GetString(2),
            LicenseNumber = reader.GetString(3),
            CurrentVehicleId = reader.IsDBNull(4) ? null : reader.GetInt64(4),
            Rating = reader.GetDecimal(5),
        };
    }

    public async Task SetCurrentVehicleAsync(
        (long DriverId, long VehicleId) updates,
        CancellationToken cancellationToken)
    {
        const string sql = """
                           update drivers
                           set current_vehicle_id = @vehicle_id
                           where driver_id = @driver_id;
                           """;

        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("driver_id", updates.DriverId);
        command.Parameters.AddWithValue("vehicle_id", updates.VehicleId);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task DeleteDriverAsync(
        long driverId,
        CancellationToken cancellationToken)
    {
        const string sql = "delete from drivers where driver_id = @driver_id;";

        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("driver_id", driverId);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
