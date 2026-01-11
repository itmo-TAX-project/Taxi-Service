using FluentMigrator;

namespace Infrastructure.Database.Migrations;

[Migration(5, description: "Create driver_availability enum")]
public sealed class CreateDriverAvailabilityEnum : Migration
{
    public override void Up()
    {
        Execute.Sql("""
                    CREATE TYPE driver_availability AS ENUM (
                        'idle',
                        'searching',
                        'busy',
                        'offline'
                    );
                    """);
    }

    public override void Down()
    {
        Execute.Sql("""
                    DROP TYPE IF EXISTS driver_availability;
                    """);
    }
}