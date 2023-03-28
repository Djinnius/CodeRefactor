using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using StaticUtilities.Objects;

namespace StaticUtilities.Utilities;

/// <summary>
///     Contains methods to provide current coordinate sensor readings.
/// </summary>
public static class CoordinateProvider
{
    private static readonly Random _random = new Random();
    private static readonly double earthRadiusInKm = 6371; // Earth's radius in km
    private static readonly double circleRadiusInKm = 10; // radius in km on the surface of a sphere





    public static Coordinate GetSensorCoordinate1(double currentAngleInRadians, double centreLatitudeInDegrees, double centreLongitudeInDegrees)
    {
        var currentCoordinate = GetCurrentCoordinate(currentAngleInRadians, centreLatitudeInDegrees, centreLongitudeInDegrees);
        return new Coordinate(currentCoordinate.Latitude + ((_random.NextDouble() - 0.5) * 0.001), currentCoordinate.Longitude + ((_random.NextDouble() - 0.5) * 0.001));
    }

    public static Coordinate GetSensorCoordinate2(double currentAngleInRadians, double centreLatitudeInDegrees, double centreLongitudeInDegrees)
    {
        var currentCoordinate = GetCurrentCoordinate(currentAngleInRadians, centreLatitudeInDegrees, centreLongitudeInDegrees);
        return new Coordinate(currentCoordinate.Latitude + ((_random.NextDouble() - 0.5) * 0.002), currentCoordinate.Longitude + ((_random.NextDouble() - 0.5) * 0.002));
    }

    public static Coordinate GetSensorCoordinate3(double currentAngleInRadians, double centreLatitudeInDegrees, double centreLongitudeInDegrees)
    {
        var currentCoordinate = GetCurrentCoordinate(currentAngleInRadians, centreLatitudeInDegrees, centreLongitudeInDegrees);
        return new Coordinate(currentCoordinate.Latitude + ((_random.NextDouble() - 0.5) * 0.003), currentCoordinate.Longitude + ((_random.NextDouble() - 0.5) * 0.003));
    }










    private static Coordinate GetCurrentCoordinate(double currentAngleInRadians, double centreLatitudeInDegrees, double centreLongitudeInDegrees)
    {
        IAppCache cache = new CachingService();

        var coordinate = cache.GetOrAdd("CurrentCoordinate", () => GetCurrentCoordinateInner(), new MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(200) });

        System.Diagnostics.Debug.WriteLine(coordinate.ToString());

        return coordinate;

        Coordinate GetCurrentCoordinateInner()
        {
            var centreLatitudeInRadians = ConvertToRadians(centreLatitudeInDegrees);
            var cantreLongitudeInRadians = ConvertToRadians(centreLongitudeInDegrees);
            var circleRadiusToEarthRadiusRatio = circleRadiusInKm / earthRadiusInKm;

            // Calculate new latitude and longitude values based on angle and circle radius using the Haversine formula
            // to determine the coordinates on the surface of a sphere of radius equal to Earth's radius
            var newLatitudeInRadians = Math.Asin(Math.Sin(centreLatitudeInRadians) * Math.Cos(circleRadiusToEarthRadiusRatio) +
                Math.Cos(centreLatitudeInRadians) * Math.Sin(circleRadiusToEarthRadiusRatio) * Math.Cos(currentAngleInRadians));

            var newLongitudeInRadians = cantreLongitudeInRadians + Math.Atan2(Math.Sin(currentAngleInRadians) * Math.Sin(circleRadiusToEarthRadiusRatio) * Math.Cos(centreLatitudeInRadians),
                Math.Cos(circleRadiusToEarthRadiusRatio) - Math.Sin(centreLatitudeInRadians) * Math.Sin(newLatitudeInRadians));

            var newLatitudeInDegrees = ConvertToDegrees(newLatitudeInRadians);
            var newLongitudeInDegrees = ConvertToDegrees(newLongitudeInRadians);
            var coordinate = new Coordinate(newLatitudeInDegrees, newLongitudeInDegrees);

            return coordinate;
        }
    }

    private static double ConvertToRadians(double angle) => (angle * Math.PI) / 180;
    private static double ConvertToDegrees(double angle) => (angle * 180) / Math.PI;
}
