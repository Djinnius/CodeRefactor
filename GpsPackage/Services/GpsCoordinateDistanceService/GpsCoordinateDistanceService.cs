using GpsPackage.Options;
using Microsoft.Extensions.Options;

namespace GpsPackage.Services.GpsCoordinateDistanceService;

/// <inheritdoc cref="IGpsCoordinateDistanceService"/>
internal class GpsCoordinateDistanceService : IGpsCoordinateDistanceService
{
    private readonly GlobeOptions _globeOptions;

    public GpsCoordinateDistanceService(IOptions<GlobeOptions> globeOptions)
    {
        _globeOptions = globeOptions.Value;
    }

    public double GetDistanceBetweenTwoCoordinatesInKilometers(Coordinate coordinate1, Coordinate coordinate2)
    {
        // Calculate the distance between two coordinates using the Haversine formula
        var dLat = (coordinate2.Latitude - coordinate1.Latitude) * Math.PI / 180;
        var dLon = (coordinate2.Longitude - coordinate1.Longitude) * Math.PI / 180;
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(coordinate1.Latitude * Math.PI / 180) * Math.Cos(coordinate2.Latitude * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = _globeOptions.RadiusOfGlobeInKm * c;
        return distance;
    }
}
