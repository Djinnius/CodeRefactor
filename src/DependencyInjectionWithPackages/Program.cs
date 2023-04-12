using DependencyInjectionWithPackages.Options;
using DependencyInjectionWithPackages.Services.CircleTracker;
using DependencyInjectionWithPackages.Services.CurrentCoordinateProvider;
using DependencyInjectionWithPackages.Services.GpsInstrumentationProvider;
using DependencyInjectionWithPackages.Services.SpeedLimitCalculator;
using DependencyInjectionWithPackages.Services.SpeedLimitProviders;
using GpsPackage.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PseudoRandom.Package.DependencyInjection;
using Timing.Package.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add API services
builder.Services.AddSingleton<ICircleTracker, CircleTracker>();

builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ISpeedLimitProvider, EnvironmentalSpeedLimitProvider>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ISpeedLimitProvider, PortSpeedLimitProvider>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ISpeedLimitProvider, WhaleSpeedLimitProvider>());

builder.Services.AddSingleton<IGpsInstrumentationProvider, GpsInstrumentationProvider>();
builder.Services.AddSingleton<ICurrentCoordinateProvider, CurrentCoordinateProvider>();
builder.Services.Decorate<ICurrentCoordinateProvider, CurrentCoordinateProviderLoggingDecorator>();
builder.Services.AddSingleton<ISpeedLimitCalculator, SpeedLimitCalculator>();

builder.Services.Configure<CircleOptions>(builder.Configuration.GetSection(CircleOptions.SectionName));
builder.Services.Configure<SpeedLimitOptions>(builder.Configuration.GetSection(SpeedLimitOptions.SectionName));

// Add package services
builder.Services.AddPseudoRandomServices(builder.Configuration);
builder.Services.AddTimingServices(builder.Configuration);
builder.Services.AddGpsServices(builder.Configuration);
builder.Services.AddLazyCache();

// Add end points
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Debug() // Recommend Seq for local logging (docker image available).
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
