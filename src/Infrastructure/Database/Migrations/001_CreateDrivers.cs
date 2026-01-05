using FluentMigrator;

namespace Infrastructure.Database.Migrations;

[Migration(1, description: "Create drivers table")]
public class CreateDrivers : Migration
{
    public override void Up()
    {
        Execute.Sql("""
                    CREATE TABLE drivers (
                        driver_id           UUID PRIMARY KEY,
                        account_id          UUID NOT NULL UNIQUE,
                        name                VARCHAR(100) NOT NULL,
                        license_number      VARCHAR(50) NOT NULL UNIQUE,
                        current_vehicle_id  UUID,
                        rating              NUMERIC(3,2) DEFAULT 5.00,
                        created_at          TIMESTAMP NOT NULL DEFAULT now()
                    );
                    """);
    }

    public override void Down()
    {
        Execute.Sql("""
                    drop table if exists order_history cascade;
                    """);
    }
}