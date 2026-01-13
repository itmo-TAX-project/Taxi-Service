using Application.DTO;
using TaxiMaster.Grpc;

namespace Presentation.Grpc.Mappers;

public static class GrpcMapper
{
    public static GetDriverResponse ToGrpcResponse(
        DriverDto driver,
        IEnumerable<Application.DTO.Enums.VehicleSegment> allowedSegments)
    {
        var grpcDriver = new Driver
        {
            DriverId = driver.DriverId,
            AccountId = driver.AccountId,
            Name = driver.Name,
            LicenseNumber = driver.LicenseNumber,
            Rating = (double)driver.Rating,
            CurrentVehicleId = driver.CurrentVehicleId ?? 0,
        };

        grpcDriver.AllowedSegments.AddRange(allowedSegments.Select(MapSegment));

        return new GetDriverResponse
        {
            Driver = grpcDriver,
        };
    }

    private static VehicleSegment MapSegment(Application.DTO.Enums.VehicleSegment segment)
    {
        return segment switch
        {
            Application.DTO.Enums.VehicleSegment.Basic => VehicleSegment.Basic,
            Application.DTO.Enums.VehicleSegment.Mid => VehicleSegment.Mid,
            Application.DTO.Enums.VehicleSegment.Premium => VehicleSegment.Premium,
            _ => VehicleSegment.Unspecified,
        };
    }
}