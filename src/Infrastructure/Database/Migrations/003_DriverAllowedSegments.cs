using FluentMigrator;

namespace Infrastructure.Database.Migrations;

[Migration(3, description: "Create driver_allowed_segments table")]
public class DriverAllowedSegments : Migration {
    public override void Up()
    {
        Execute.Sql("""
                    CREATE TABLE driver_allowed_segments (
                        driver_id UUID NOT NULL REFERENCES drivers(driver_id) ON DELETE CASCADE,
                        segment   VARCHAR(10) NOT NULL CHECK (segment IN ('basic','mid','premium')),
                        PRIMARY KEY (driver_id, segment)
                    );
                    """);
    }

    public override void Down()
    {
        Execute.Sql("""
                    drop table if exists driver_allowed_segments cascade;
                    """);
    }
}