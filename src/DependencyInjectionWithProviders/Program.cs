using DependencyInjectionWithProviders.Providers.Clock;
using DependencyInjectionWithProviders.Providers.RandomProvider;
using DependencyInjectionWithProviders.Services.CoordinateAggregator;
using DependencyInjectionWithProviders.Services.CoordinateProvider;
using DependencyInjectionWithProviders.Services.SpeedLimitCalculator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLazyCache();

builder.Services.AddSingleton<ICoordinateProvider, CoordinateProvider>();
builder.Services.AddSingleton<ICoordinateAggregator, CoordinateAggregator>();
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
