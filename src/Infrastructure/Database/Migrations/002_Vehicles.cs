using FluentMigrator;

namespace Infrastructure.Database.Migrations;

[Migration(2, description: "Create vehicles table")]
public class Vehicles : Migration
{
    public override void Up()
    {
        Execute.Sql("""
                    CREATE TABLE vehicles (
                        vehicle_id  UUID PRIMARY KEY,
                        driver_id   UUID NOT NULL REFERENCES drivers(driver_id) ON DELETE CASCADE,
                        segment     VARCHAR(10) NOT NULL CHECK (segment IN ('basic','mid','premium')),
                        plate       VARCHAR(20) NOT NULL UNIQUE,
                        model       VARCHAR(100) NOT NULL,
                        capacity    INT NOT NULL CHECK (capacity > 0),
                        created_at  TIMESTAMP NOT NULL DEFAULT now()
                    );
                    """);
    }

    public override void Down()
    {
        Execute.Sql("""
                    drop table if exists vehicles cascade;
                    """);
    }
}