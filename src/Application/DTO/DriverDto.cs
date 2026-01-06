namespace Application.DTO;

public class DriverDto
{
    public long DriverId { get; init; }

    public long AccountId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string LicenseNumber { get; init; } = string.Empty;

    public long? CurrentVehicleId { get; init; }

    public decimal Rating { get; init; }
}