using Application.DTO.Enums;
using Itmo.Dev.Platform.Persistence.Postgres.Plugins;
using Npgsql;

namespace Infrastructure.Extensions.Plugins;

public class DriverSegmentMappingPlugin : IPostgresDataSourcePlugin
{
    public void Configure(NpgsqlDataSourceBuilder dataSource)
    {
        dataSource.MapEnum<VehicleSegment>("vehicle_segment");
    }
}