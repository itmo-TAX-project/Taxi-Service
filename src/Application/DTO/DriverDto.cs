namespace Application.DTO;

public class DriverDto
{
    public Guid DriverId { get; init; }

    public Guid AccountId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string LicenseNumber { get; init; } = string.Empty;

    public Guid? CurrentVehicleId { get; init; }

    public decimal Rating { get; init; }
}