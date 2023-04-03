using DependencyInjectionWithPackages.Options;
using DependencyInjectionWithPackages.Providers.RandomProvider;
using DependencyInjectionWithPackages.Services.CircleTracker;
using GpsPackage;
using GpsPackage.Options;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace DependencyInjectionWithPackages.Services.GpsInstrumentationProvider;

/// <inheritdoc cref="IGpsInstrumentationProvider"/>
public sealed class GpsInstrumentationProvider : IGpsInstrumentationProvider
{
    private readonly IAppCache _appCache;
    private readonly ILogger<GpsInstrumentationProvider> _logger;
    private readonly IRandomProvider _randomProvider;
    private readonly ICircleTracker _circleTracker;
    private readonly CircleOptions _circleOptions;
    private readonly GlobeOptions _globeOptions;

    /// <inheritdoc cref="GpsInstrumentationProvider"/>
    public GpsInstrumentationProvider(
        IAppCache appCache,
        ILogger<GpsInstrumentationProvider> logger,
        IRandomProvider randomProvider,
        ICircleTracker circleTracker,
        IOptions<GlobeOptions> globeOptions,
        IOptions<CircleOptions> circleOptions)
    {
        _appCache = appCache;
        _logger = logger;
        _randomProvider = randomProvider;
        _circleTracker = circleTracker;
        _circleOptions = circleOptions.Value;
        _globeOptions = globeOptions.Value;
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
            var circleRadiusToEarthRadiusRatio = _circleOptions.CircleRadiusInKm / _globeOptions.RadiusOfGlobeInKm;

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
