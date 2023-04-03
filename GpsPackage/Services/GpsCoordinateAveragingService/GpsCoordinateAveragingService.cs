using GpsPackage.Services.GpsCoordinateDistanceService;

namespace GpsPackage.Services.GpsCoordinateAveragingService;

/// <inheritdoc cref="IGpsCoordinateAveragingService"/>
internal sealed class GpsCoordinateAveragingService : IGpsCoordinateAveragingService
{
    private readonly IGpsCoordinateDistanceService _gpsCoordinateDistanceService;

    public GpsCoordinateAveragingService(IGpsCoordinateDistanceService gpsCoordinateDistanceService)
    {
        _gpsCoordinateDistanceService = gpsCoordinateDistanceService;
    }

    public Coordinate GetAverageCoordinateWithErrorCorrection(Coordinate coordinate1, Coordinate coordinate2, Coordinate coordinate3)
    {
        var dist12 = _gpsCoordinateDistanceService.GetDistanceBetweenTwoCoordinatesInKilometers(coordinate1, coordinate2);
        var dist13 = _gpsCoordinateDistanceService.GetDistanceBetweenTwoCoordinatesInKilometers(coordinate1, coordinate3);
        var dist23 = _gpsCoordinateDistanceService.GetDistanceBetweenTwoCoordinatesInKilometers(coordinate2, coordinate3);

        if (HasErroneousCoordinate(dist12, dist13, dist23))
        {
            var minDist = Math.Min(dist12, Math.Min(dist13, dist23));
            if (minDist == dist12) return coordinate1.AverageWith(coordinate2);
            if (minDist == dist13) return coordinate1.AverageWith(coordinate3);
            return coordinate2.AverageWith(coordinate3);
        }

        return coordinate1.AverageWith(coordinate2, coordinate3);
    }

    /// <summary>
    ///     If the longest distance is more than 10% larger than the average of the other two distances, 
    ///     consider it a faulty reading.
    /// </summary>
    /// <param name="maxDist"> The maximum distance. </param>
    /// <param name="dist1"> The first distance. </param>
    /// <param name="dist2"> The second distance. </param>
    /// <param name="dist3"> The first distance. </param>
    /// <returns> <see langword="true"/> if the distances indicate a faulty coordinate reading, otherwise <see langword="false"/>. </returns>
    private static bool HasErroneousCoordinate(double dist1, double dist2, double dist3)
    {
        var maxDist = Math.Max(dist1, Math.Max(dist2, dist3));
        return maxDist > 1.1 * (dist1 + dist2 + dist3 - maxDist) / 2;
    }
}
