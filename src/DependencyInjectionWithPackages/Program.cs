using DependencyInjectionWithPackages.Options;
using DependencyInjectionWithPackages.Providers.Clock;
using DependencyInjectionWithPackages.Providers.RandomProvider;
using DependencyInjectionWithPackages.Services.CircleTracker;
using DependencyInjectionWithPackages.Services.CurrentCoordinateProvider;
using DependencyInjectionWithPackages.Services.GpsInstrumentationProvider;
using DependencyInjectionWithPackages.Services.SpeedLimitCalculator;
using DependencyInjectionWithPackages.Services.SpeedLimitProviders;
using GpsPackage.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLazyCache();

builder.Services.AddSingleton<ICircleTracker, CircleTracker>();

builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ISpeedLimitProvider, EnvironmentalSpeedLimitProvider>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ISpeedLimitProvider, PortSpeedLimitProvider>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ISpeedLimitProvider, WhaleSpeedLimitProvider>());

builder.Services.AddSingleton<IGpsInstrumentationProvider, GpsInstrumentationProvider>();
builder.Services.AddSingleton<ICurrentCoordinateProvider, CurrentCoordinateProvider>();
builder.Services.AddSingleton<ISpeedLimitCalculator, SpeedLimitCalculator>();

builder.Services.AddSingleton<IRandomProvider, RandomProvider>();
builder.Services.AddSingleton<IClock, Clock>();

builder.Services.Configure<CircleOptions>(builder.Configuration.GetSection(CircleOptions.SectionName));
builder.Services.Configure<SpeedLimitOptions>(builder.Configuration.GetSection(SpeedLimitOptions.SectionName));

builder.Services.AddGpsServices(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
