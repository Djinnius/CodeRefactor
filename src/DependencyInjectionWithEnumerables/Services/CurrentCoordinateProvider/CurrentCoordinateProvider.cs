using DependencyInjectionWithOptions.Objects;
using DependencyInjectionWithOptions.Providers.RandomProvider;
using DependencyInjectionWithOptions.Services.CircleTracker;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;

namespace DependencyInjectionWithOptions.Services.CurrentCoordinateProvider;

/// <inheritdoc cref="ICurrentCoordinateProvider"/>
public class CurrentCoordinateProvider : ICurrentCoordinateProvider
{
    private static readonly double earthRadiusInKm = 6371; // Earth's radius in km
    private static readonly double circleRadiusInKm = 10; // radius in km on the surface of a sphere

    private readonly IAppCache _appCache;
    private readonly ILogger<CurrentCoordinateProvider> _logger;
    private readonly IRandomProvider _randomProvider;
    private readonly ICircleTracker _circleTracker;

    /// <inheritdoc cref="CurrentCoordinateProvider"/>
    public CurrentCoordinateProvider(IAppCache appCache, ILogger<CurrentCoordinateProvider> logger, IRandomProvider randomProvider, ICircleTracker circleTracker)
    {
        _appCache = appCache;
        _logger = logger;
        _randomProvider = randomProvider;
        _circleTracker = circleTracker;
    }

    public Coordinate GetSensorCoordinate1()
    {
        var currentCoordinate = GetCurrentCoordinate();
        return new Coordinate(currentCoordinate.Latitude + (_randomProvider.Random.NextDouble() - 0.5) * 0.001, currentCoordinate.Longitude + (_randomProvider.Random.NextDouble() - 0.5) * 0.001);
    }

    public Coordinate GetSensorCoordinate2()
    {
        var currentCoordinate = GetCurrentCoordinate();
        return new Coordinate(currentCoordinate.Latitude + (_randomProvider.Random.NextDouble() - 0.5) * 0.002, currentCoordinate.Longitude + (_randomProvider.Random.NextDouble() - 0.5) * 0.002);
    }

    public Coordinate GetSensorCoordinate3()
    {
        var currentCoordinate = GetCurrentCoordinate();
        return new Coordinate(currentCoordinate.Latitude + (_randomProvider.Random.NextDouble() - 0.5) * 0.003, currentCoordinate.Longitude + (_randomProvider.Random.NextDouble() - 0.5) * 0.003);
    }







    #region private

    /// <summary>
    ///     Placeholder for instrumentation system.
    /// </summary>
    /// <returns> The current coordinate </returns>
    private Coordinate GetCurrentCoordinate()
    {
        var coordinate = _appCache.GetOrAdd("CurrentCoordinate", () => GetCurrentCoordinateInner(), new MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(200) });

        _logger.LogInformation(coordinate.ToString());

        return coordinate;

        Coordinate GetCurrentCoordinateInner()
        {
            var centreLatitudeInRadians = ConvertToRadians(_circleTracker.CircleCentreCoordinate.Latitude);
            var cantreLongitudeInRadians = ConvertToRadians(_circleTracker.CircleCentreCoordinate.Longitude);
            var circleRadiusToEarthRadiusRatio = circleRadiusInKm / earthRadiusInKm;

            // Calculate new latitude and longitude values based on angle and circle radius using the Haversine formula
            // to determine the coordinates on the surface of a sphere of radius equal to Earth's radius
            var newLatitudeInRadians = Math.Asin(Math.Sin(centreLatitudeInRadians) * Math.Cos(circleRadiusToEarthRadiusRatio) +
                Math.Cos(centreLatitudeInRadians) * Math.Sin(circleRadiusToEarthRadiusRatio) * Math.Cos(_circleTracker.CurrentAngleInRadians));

            var newLongitudeInRadians = cantreLongitudeInRadians + Math.Atan2(Math.Sin(_circleTracker.CurrentAngleInRadians) * Math.Sin(circleRadiusToEarthRadiusRatio) * Math.Cos(centreLatitudeInRadians),
                Math.Cos(circleRadiusToEarthRadiusRatio) - Math.Sin(centreLatitudeInRadians) * Math.Sin(newLatitudeInRadians));

            var newLatitudeInDegrees = ConvertToDegrees(newLatitudeInRadians);
            var newLongitudeInDegrees = ConvertToDegrees(newLongitudeInRadians);
            var coordinate = new Coordinate(newLatitudeInDegrees, newLongitudeInDegrees);

            return coordinate;
        }
    }

    private static double ConvertToRadians(double angle) => angle * Math.PI / 180;
    private static double ConvertToDegrees(double angle) => angle * 180 / Math.PI;

    #endregion
}
