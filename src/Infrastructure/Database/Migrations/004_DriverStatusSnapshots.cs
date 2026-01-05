using FluentMigrator;

namespace Infrastructure.Database.Migrations;

[Migration(4, description: "Create driver_status_snapshots table")]
public class DriverStatusSnapshots : Migration {
    public override void Up()
    {
        Execute.Sql("""
                    CREATE TABLE driver_status_snapshots (
                        id            BIGSERIAL PRIMARY KEY,
                        driver_id     UUID NOT NULL REFERENCES drivers(driver_id) ON DELETE CASCADE,
                        latitude      DOUBLE PRECISION NOT NULL,
                        longitude     DOUBLE PRECISION NOT NULL,
                        availability  VARCHAR(10) NOT NULL CHECK (availability IN ('idle','searching','busy')),
                        ts            TIMESTAMP NOT NULL DEFAULT now()
                    );
                    CREATE INDEX idx_driver_status_driver_id ON driver_status_snapshots(driver_id);
                    CREATE INDEX idx_driver_status_ts ON driver_status_snapshots(ts DESC);
                    """);
    }

    public override void Down()
    {
        Execute.Sql("""
                    drop table if exists driver_status_snapshots cascade;
                    """);
    }
}