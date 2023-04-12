using DependencyInjectionWithPackages.Options;
using DependencyInjectionWithPackages.Services.CircleTracker;
using GpsPackage.DataObjects;
using GpsPackage.Options;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PseudoRandom.Package.Randomness;

namespace DependencyInjectionWithPackages.Services.GpsInstrumentationProvider;

/// <inheritdoc cref="IGpsInstrumentationProvider"/>
public sealed class GpsInstrumentationProvider : IGpsInstrumentationProvider
{
    private readonly IAppCache _appCache;
    private readonly ILogger<GpsInstrumentationProvider> _logger;
    private readonly IRandomNumberProvider _randomProvider;
    private readonly ICircleTracker _circleTracker;
    private readonly CircleOptions _circleOptions;
    private readonly GlobeOptions _globeOptions;

    /// <inheritdoc cref="GpsInstrumentationProvider"/>
    public GpsInstrumentationProvider(
        IAppCache appCache,
        ILogger<GpsInstrumentationProvider> logger,
        IRandomNumberProvider randomProvider,
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
        return new Coordinate(currentCoordinate.Latitude + _randomProvider.GetNextDoubleBetween(-0.005, 0.005), currentCoordinate.Longitude + _randomProvider.GetNextDoubleBetween(-0.005, 0.005));
    }

    public Coordinate GetSensorCoordinate2()
    {
        var currentCoordinate = GetCurrentCoordinate();
        return new Coordinate(currentCoordinate.Latitude + _randomProvider.GetNextDoubleBetween(-0.01, 0.01), currentCoordinate.Longitude + _randomProvider.GetNextDoubleBetween(-0.01, 0.01));
    }

    public Coordinate GetSensorCoordinate3()
    {
        var currentCoordinate = GetCurrentCoordinate();
        return new Coordinate(currentCoordinate.Latitude + _randomProvider.GetNextDoubleBetween(-0.015, 0.015), currentCoordinate.Longitude + _randomProvider.GetNextDoubleBetween(-0.015, 0.015));
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
            var centreLatitudeInRadians = ConvertToRadians(_circleOptions.CircleCentreCoordinate.Latitude);
            var centreLongitudeInRadians = ConvertToRadians(_circleOptions.CircleCentreCoordinate.Longitude);
            var circleRadiusToGlobeRadiusRatio = _circleOptions.CircleRadiusInKm / _globeOptions.RadiusOfGlobeInKm;

            // Calculate new latitude and longitude values based on angle and circle radius using the Haversine formula
            // to determine the coordinates on the surface of a sphere of radius equal to the provided globe.
            var newLatitudeInRadians = Math.Asin(Math.Sin(centreLatitudeInRadians) * Math.Cos(circleRadiusToGlobeRadiusRatio) +
                Math.Cos(centreLatitudeInRadians) * Math.Sin(circleRadiusToGlobeRadiusRatio) * Math.Cos(_circleTracker.CurrentAngleInRadians));

            var newLongitudeInRadians = centreLongitudeInRadians + Math.Atan2(Math.Sin(_circleTracker.CurrentAngleInRadians) * Math.Sin(circleRadiusToGlobeRadiusRatio) * Math.Cos(centreLatitudeInRadians),
                Math.Cos(circleRadiusToGlobeRadiusRatio) - Math.Sin(centreLatitudeInRadians) * Math.Sin(newLatitudeInRadians));

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
