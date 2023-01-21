using GrpcTraining.Clients;
using GrpcTraining.Resources.OpenDota;
using GrpcTraining.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddOptions();
builder.Services.Configure<OpenDotaConfig>(builder.Configuration.GetSection("OpenDotaConfig"));

builder.Services.AddHttpClient();
builder.Services.AddTransient<IOpenDotaClient, OpenDotaClient>();

var app = builder.Build();

app.MapGrpcService<DotaService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
