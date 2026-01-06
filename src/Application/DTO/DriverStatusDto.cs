using Application.DTO.Enums;

namespace Application.DTO;

public class DriverStatusDto
{
    public Guid DriverId { get; init; }

    public double Latitude { get; init; }

    public double Longitude { get; init; }

    public DriverAvailability Availability { get; init; }

    public DateTime Timestamp { get; init; }
}