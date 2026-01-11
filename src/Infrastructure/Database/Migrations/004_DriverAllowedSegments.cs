using FluentMigrator;

namespace Infrastructure.Database.Migrations;

[Migration(4, description: "Create driver_allowed_segments table")]
public class DriverAllowedSegments : Migration
{
    public override void Up()
    {
        Execute.Sql("""
                    CREATE TABLE driver_allowed_segments (
                        driver_id   BIGINT NOT NULL REFERENCES drivers(driver_id) ON DELETE CASCADE,
                        segment     vehicle_segment NOT NULL,
                        PRIMARY     KEY (driver_id, segment)
                    );
                    """);
    }

    public override void Down()
    {
        Execute.Sql("""
                    DROP TABLE IF EXISTS driver_allowed_segments CASCADE;
                    """);
    }
}
