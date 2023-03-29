using DependencyInjectionWithOptions.Options;
using DependencyInjectionWithOptions.Providers.Clock;
using DependencyInjectionWithOptions.Providers.RandomProvider;
using DependencyInjectionWithOptions.Services.CircleTracker;
using DependencyInjectionWithOptions.Services.CurrentCoordinateProvider;
using DependencyInjectionWithOptions.Services.GpsInstrumentationProvider;
using DependencyInjectionWithOptions.Services.SpeedLimitCalculator;
using DependencyInjectionWithOptions.Services.SpeedLimitProviders;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

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
builder.Services.Configure<GlobeOptions>(builder.Configuration.GetSection(GlobeOptions.SectionName));
builder.Services.Configure<SpeedLimitOptions>(builder.Configuration.GetSection(SpeedLimitOptions.SectionName));

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
