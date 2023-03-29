using DependencyInjectionWithOptions.Services.CoordinateAggregator;
using DependencyInjectionWithOptions.Services.CoordinateProvider;
using DependencyInjectionWithOptions.Services.SpeedLimitCalculator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLazyCache();

builder.Services.AddSingleton<ICoordinateProvider, CoordinateProvider>();
builder.Services.AddSingleton<ICoordinateAggregator, CoordinateAggregator>();
builder.Services.AddSingleton<ISpeedLimitCalculator, SpeedLimitCalculator>();

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
