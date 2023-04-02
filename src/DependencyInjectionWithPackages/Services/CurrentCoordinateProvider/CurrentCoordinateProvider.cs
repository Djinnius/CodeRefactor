using DependencyInjectionWithPackages.Objects;
using DependencyInjectionWithPackages.Options;
using DependencyInjectionWithPackages.Services.GpsInstrumentationProvider;
using Microsoft.Extensions.Options;

namespace DependencyInjectionWithPackages.Services.CurrentCoordinateProvider;

/// <inheritdoc cref="ICurrentCoordinateProvider"/>
public class CurrentCoordinateProvider : ICurrentCoordinateProvider
{
    private readonly IGpsInstrumentationProvider _gpsInstrumentationProvider;
    private readonly GlobeOptions _globeOptions;

    /// <inheritdoc cref="CurrentCoordinateProvider"/>
    public CurrentCoordinateProvider(IGpsInstrumentationProvider gpsInstrumentationProvider, IOptions<GlobeOptions> globeOptions)
    {
        _gpsInstrumentationProvider = gpsInstrumentationProvider;
        _globeOptions = globeOptions.Value;
    }

    public Coordinate GetCurrentCoordinate()
    {
        // Read three coordinates from 'instrumentation system' e.g. aeroplanes have multiple sensors in case of a fault.
        var coordinate1 = _gpsInstrumentationProvider.GetSensorCoordinate1();
        var coordinate2 = _gpsInstrumentationProvider.GetSensorCoordinate2();
        var coordinate3 = _gpsInstrumentationProvider.GetSensorCoordinate3();

        // Calculate the 'average' coordinate with protection from a single outlier
        Coordinate coordinate;

        // Calculate distances between each pair of coordinates
        double dist1 = CalculateDistance(coordinate1, coordinate2);
        double dist2 = CalculateDistance(coordinate1, coordinate3);
        double dist3 = CalculateDistance(coordinate2, coordinate3);

        // Determine which distance is the maximum/minimum
        double maxDist = Math.Max(dist1, Math.Max(dist2, dist3));
        double minDist = Math.Min(dist1, Math.Min(dist2, dist3));

        // If the longest distance is more than 10% larger than the average of the other two distances, consider it a faulty reading
        if (maxDist > 1.1 * (dist1 + dist2 + dist3 - maxDist) / 2)
        {
            // Ignore the coordinate that contributed to the longest distance and recalculate the average based on the minimum distance pair
            if (minDist == dist1)
                coordinate = new Coordinate((coordinate1.Latitude + coordinate2.Latitude) / 2, (coordinate1.Longitude + coordinate2.Longitude) / 2);
            else if (minDist == dist2)
                coordinate = new Coordinate((coordinate1.Latitude + coordinate3.Latitude) / 2, (coordinate1.Longitude + coordinate3.Longitude) / 2);
            else
                coordinate = new Coordinate((coordinate2.Latitude + coordinate3.Latitude) / 2, (coordinate2.Longitude + coordinate3.Longitude) / 2);
        }
        else
        {
            // Calculate the average of the three coordinates
            coordinate = new Coordinate((coordinate1.Latitude + coordinate2.Latitude + coordinate3.Latitude) / 3, (coordinate1.Longitude + coordinate2.Longitude + coordinate3.Longitude) / 3);
        }

        return coordinate;
    }

    private double CalculateDistance(Coordinate coordinate1, Coordinate coordinate2)
    {
        // Calculate the distance between two coordinates using the Haversine formula
        double dLat = (coordinate2.Latitude - coordinate1.Latitude) * Math.PI / 180;
        double dLon = (coordinate2.Longitude - coordinate1.Longitude) * Math.PI / 180;
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(coordinate1.Latitude * Math.PI / 180) * Math.Cos(coordinate2.Latitude * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = _globeOptions.RadiusOfGlobeInKm * c;
        return distance;
    }
}
