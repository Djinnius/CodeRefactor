using DependencyInjectionWithEnumerables.Providers.Clock;
using DependencyInjectionWithEnumerables.Providers.RandomProvider;
using DependencyInjectionWithEnumerables.Services.CircleTracker;
using DependencyInjectionWithEnumerables.Services.CurrentCoordinateAggregator;
using DependencyInjectionWithEnumerables.Services.CurrentCoordinateProvider;
using DependencyInjectionWithEnumerables.Services.SpeedLimitCalculator;
using DependencyInjectionWithEnumerables.Services.SpeedLimitProviders;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLazyCache();

builder.Services.AddSingleton<ICircleTracker, CircleTracker>();

builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ISpeedLimitProvider, EnvironmentalSpeedLimitProvider>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ISpeedLimitProvider, PortSpeedLimitProvider>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ISpeedLimitProvider, WhaleSpeedLimitProvider>());

builder.Services.AddSingleton<ICurrentCoordinateProvider, CurrentCoordinateProvider>();
builder.Services.AddSingleton<ICurrentCoordinateAggregator, CurrentCoordinateAggregator>();
builder.Services.AddSingleton<ISpeedLimitCalculator, SpeedLimitCalculator>();

builder.Services.AddSingleton<IRandomProvider, RandomProvider>();
builder.Services.AddSingleton<IClock, Clock>();

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
