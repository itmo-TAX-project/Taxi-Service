using FluentMigrator;

namespace Infrastructure.Database.Migrations;

[Migration(6, description: "Create driver_status_snapshots table")]
public sealed class CreateDriverStatusSnapshots : Migration
{
    public override void Up()
    {
        Execute.Sql("""
                    CREATE TABLE driver_status_snapshots (
                        id              BIGSERIAL PRIMARY KEY,
                        driver_id       BIGINT NOT NULL REFERENCES drivers(driver_id) ON DELETE CASCADE,
                        latitude        DOUBLE PRECISION NOT NULL,
                        longitude       DOUBLE PRECISION NOT NULL,
                        availability    driver_availability NOT NULL,
                        ts              TIMESTAMP NOT NULL DEFAULT now()
                    );

                    CREATE INDEX idx_driver_status_driver_id
                        ON driver_status_snapshots(driver_id);

                    CREATE INDEX idx_driver_status_ts
                        ON driver_status_snapshots(ts DESC);
                    """);
    }

    public override void Down()
    {
        Execute.Sql("""
                    DROP TABLE IF EXISTS driver_status_snapshots CASCADE;
                    """);
    }
}
