using GrpcTraining.Clients;
using GrpcTraining.Resources.OpenDota;
using GrpcTraining.Services;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);


ConfigureLogs();

builder.Host.UseSerilog();

builder.Services.AddGrpc();

builder.Services.AddOptions();
builder.Services.Configure<OpenDotaConfig>(builder.Configuration.GetSection("OpenDotaConfig"));

builder.Services.AddHttpClient();
builder.Services.AddTransient<IOpenDotaClient, OpenDotaClient>();

var app = builder.Build();

app.MapGrpcService<DotaService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();


void ConfigureLogs()
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureElasticSearch(config, env))
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSearch(IConfigurationRoot config, string? env)
{
    return new ElasticsearchSinkOptions(new Uri(config["ElasticSearchConfig:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{config["ApplicationName"]}-logs-{env?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
    };
}