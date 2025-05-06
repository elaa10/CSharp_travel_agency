using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

using GrpcContracts;
using persistence;
using persistence.interfaces;
using server;
using services;


var builder = WebApplication.CreateBuilder(args);

// 1. Configurare Grpc
builder.Services.AddGrpc();

// 2. Încarcă connection string din app.config (cum aveai în ServerApp)
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var connectionString = System.Configuration.ConfigurationManager
                           .ConnectionStrings["turismDB"]?.ConnectionString
                       ?? throw new Exception("Connection string 'turismDB' not found in app.config!");

// 3. Adaugă dependențele pentru repository-uri
var dbProps = new Dictionary<string, string> { ["ConnectionString"] = connectionString };

builder.Services.AddSingleton<ISoftUserRepository>(_ => new SoftUserDBRepository(dbProps));
builder.Services.AddSingleton<ITripRepository>(_ => new TripDBRepository(dbProps));
builder.Services.AddSingleton<IReservationRepository>(provider =>
{
    var tripRepo = provider.GetRequiredService<ITripRepository>();
    return new ReservationDBRepository(dbProps, tripRepo);
});

// 4. Înregistrăm serviciul gRPC
builder.Services.AddScoped<IServices, ServicesImpl>();
builder.Services.AddScoped<ServiceImplGrpc>();

var app = builder.Build();

// 5. Mapăm serviciul gRPC
app.MapGrpcService<ServiceImplGrpc>(); // serviciul tău
app.MapGet("/", () => "This is a gRPC server."); // pentru testare

app.Run();