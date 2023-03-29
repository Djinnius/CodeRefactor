using OneOf;
using OneOf.Types;

namespace StaticUtilities.Utilities;

public static class SpeedLimitCalculator
{
    private static readonly double _centreLatitudeInDegrees = 51.5167; // Latitude of circle's center
    private static readonly double _centreLongitudeInDegrees = -0.1246; // Longitude of circle's center
    private static double _currentAngleInRadians = 0; // Current angle in radians

    private static double environmentalSpeedLimitInKmPerHour = 30; // navigate efficiently
    private static double portSpeedLimitInKmPerHour = 20; // navigate carefully in busy places
    private static double whaleSpeedLimitInKmPerHour = 15; // don't disturb whales

    public static OneOf<double, None> GetSpeedLimitForCurrentPosition()
    {
        _currentAngleInRadians += 10 * Math.PI / 180; // increment by 10 degrees

        var currentCoordinate = CoordinateAggregator.GetCurrentCoordinate(_currentAngleInRadians, _centreLatitudeInDegrees, _centreLongitudeInDegrees);
        double currentSpeedLimit = double.PositiveInfinity; // default to positive infinity for 'there is no speed limit'

        // Check speed limit due to environmental regulations (applies in top half of circle)
        if (currentCoordinate.Latitude > _centreLatitudeInDegrees && environmentalSpeedLimitInKmPerHour < currentSpeedLimit)
            currentSpeedLimit = environmentalSpeedLimitInKmPerHour;

        // Check speed limit due to port regulations (applies in right half of circle)
        if (currentCoordinate.Longitude > _centreLongitudeInDegrees && portSpeedLimitInKmPerHour < currentSpeedLimit)
            currentSpeedLimit = portSpeedLimitInKmPerHour;

        // Check speed limit due to whale regulations (applies in top right quadrant of circle in March, April and May)
        var currentDate = DateTime.UtcNow.Date;
        var currentYear = currentDate.Year;
        var startOfWhaleSeason = new DateTime(currentYear, 3, 1); // March 1st
        var endOfWhaleSeason = new DateTime(currentYear, 6, 1); // June 1st

        if (currentCoordinate.Latitude > _centreLatitudeInDegrees && currentCoordinate.Longitude > _centreLongitudeInDegrees
            && currentDate >= startOfWhaleSeason && currentDate < endOfWhaleSeason
            && whaleSpeedLimitInKmPerHour < currentSpeedLimit)
            currentSpeedLimit = whaleSpeedLimitInKmPerHour;

        if (double.IsInfinity(currentSpeedLimit))
            return new None();

        return currentSpeedLimit;
    }
}
