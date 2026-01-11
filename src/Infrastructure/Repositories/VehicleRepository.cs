using Application.DTO;
using Application.DTO.Enums;
using Application.Repositories;
using Npgsql;

namespace Infrastructure.Repositories;

public sealed class VehicleRepository(NpgsqlDataSource dataSource) : IVehicleRepository
{
    public async Task<long> AddAsync(
        VehicleDto vehicle,
        CancellationToken cancellationToken)
    {
        const string sql = """
        insert into vehicles (driver_id, segment, plate, model, capacity)
        values (@driver_id,@segment,@plate,@model,@capacity)
        returning vehicle_id;
        """;

        await using NpgsqlConnection connection =
            await dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("driver_id", vehicle.DriverId);
        command.Parameters.AddWithValue("segment", vehicle.Segment);
        command.Parameters.AddWithValue("plate", vehicle.Plate);
        command.Parameters.AddWithValue("model", vehicle.Model);
        command.Parameters.AddWithValue("capacity", vehicle.Capacity);

        return (long)(await command.ExecuteScalarAsync(cancellationToken) ??
                            throw new NullReferenceException("Could not create vehicle"));
    }

    public async Task<IEnumerable<VehicleDto>> GetByDriverAsync(long driverId, CancellationToken cancellationToken)
    {
        const string sql = """
        select vehicle_id, driver_id, segment, plate, model, capacity
        from vehicles
        where driver_id = @driver_id;
        """;

        await using NpgsqlConnection connection = await dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("driver_id", driverId);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        var vehicles = new List<VehicleDto>();

        while (await reader.ReadAsync(cancellationToken))
        {
            vehicles.Add(new VehicleDto
            {
                VehicleId = reader.GetInt64(0),
                DriverId = reader.GetInt64(1),
                Segment = reader.GetFieldValue<VehicleSegment>(2),
                Plate = reader.GetString(3),
                Model = reader.GetString(4),
                Capacity = reader.GetInt32(5),
            });
        }

        return vehicles;
    }
}
