using Application.Extensions;
using Infrastructure.Database.Options;
using Infrastructure.Extensions;
using Itmo.Dev.Platform.Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Extensions;
using Presentation.Grpc.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection("Postgres"));

builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
{
    ["Platform:ServiceName"] = "Taxi-Service",
    ["Platform:Observability:Tracing:IsEnabled"] = "false",
});

builder.Services.AddPlatform();

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddPresentation();

WebApplication app = builder.Build();

app.MapGrpcService<GrpcTaxiService>();

app.Run();