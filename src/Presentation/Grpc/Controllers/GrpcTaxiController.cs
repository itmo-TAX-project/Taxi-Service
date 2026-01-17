using Application.DTO;
using Application.Services.Interfaces;
using Grpc.Core;
using Presentation.Grpc.Mappers;
using TaxiMaster.Grpc;

namespace Presentation.Grpc.Controllers;

public class GrpcTaxiController : TaxiService.TaxiServiceBase
{
    private readonly IDriverService _driverService;
    private readonly IDriverStatusService _driverStatusService;

    public GrpcTaxiController(IDriverService driverService, IDriverStatusService driverStatusService)
    {
        _driverService = driverService;
        _driverStatusService = driverStatusService;
    }

    public override async Task<GetDriverResponse> GetDriver(GetDriverRequest request, ServerCallContext context)
    {
        DriverDto? driver = await _driverService.GetDriverAsync(request.AccountId, context.CancellationToken);

        if (driver == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Not found"));
        }

        IEnumerable<Application.DTO.Enums.VehicleSegment> vehicleSegments = await _driverService
            .GetAllowedSegmentsAsyncByDriverId(driver.DriverId, context.CancellationToken);

        return GrpcMapper.ToGrpcResponse(driver, vehicleSegments);
    }

    public override async Task<ValidateDriverResponse> ValidateDriverActive(
        ValidateDriverRequest request,
        ServerCallContext context)
    {
        bool isActive = await _driverStatusService.ValidateDriverActiveAsync(
            request.DriverId,
            context.CancellationToken);

        return new ValidateDriverResponse { IsActive = isActive };
    }
}