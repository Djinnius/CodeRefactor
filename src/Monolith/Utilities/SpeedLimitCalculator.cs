using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Monolith.Objects;
using OneOf;
using OneOf.Types;

namespace Monolith.Utilities
{
    public static class SpeedLimitCalculator
    {
        private static readonly Random _random = new Random();

        private static readonly double earthRadiusInKm = 6371; // Earth's radius in km
        private static readonly double circleRadiusInKm = 10; // radius in km on the surface of a sphere
        private static readonly double _centreLatitudeInDegrees = 51.5167; // Latitude of circle's center
        private static readonly double _centreLongitudeInDegrees = -0.1246; // Longitude of circle's center
        private static double _currentAngleInRadians = 0; // Current angle in radians

        private static double environmentalSpeedLimitInKmPerHour = 30; // navigate efficiently
        private static double portSpeedLimitInKmPerHour = 20; // navigate carefully in busy places
        private static double whaleSpeedLimitInKmPerHour = 15; // don't disturb whales

        public static OneOf<double, None> GetSpeedLimitForCurrentPosition()
        {
            // Read three coordinates from 'instrumentation system' e.g. aeroplanes have multiple sensors in case of a fault.
            var coordinate1 = GetSensorCoordinate1();
            var coordinate2 = GetSensorCoordinate2();
            var coordinate3 = GetSensorCoordinate3();

            // Calculate the 'average' coordinate with protection from a single outlier
            Coordinate coordinate;

            // Calculate distances between each pair of coordinates
            double dist1 = CalculateDistance(coordinate1, coordinate2);
            double dist2 = CalculateDistance(coordinate1, coordinate3);
            double dist3 = CalculateDistance(coordinate2, coordinate3);

            // Determine which distance is the maximum
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

            double currentSpeedLimit = double.PositiveInfinity; // default to positive infinity for 'there is no speed limit'

            // Check speed limit due to environmental regulations (applies in top half of circle)
            if (coordinate.Latitude > _centreLatitudeInDegrees && environmentalSpeedLimitInKmPerHour < currentSpeedLimit)
                currentSpeedLimit = environmentalSpeedLimitInKmPerHour;

            // Check speed limit due to port regulations (applies in right half of circle)
            if (coordinate.Longitude > _centreLongitudeInDegrees && portSpeedLimitInKmPerHour < currentSpeedLimit)
                currentSpeedLimit = portSpeedLimitInKmPerHour;

            // Check speed limit due to whale regulations (applies in top right quadrant of circle in March, April and May)
            var currentDate = DateTime.UtcNow.Date;
            var currentYear = currentDate.Year;
            var startOfWhaleSeason = new DateTime(currentYear, 3, 1); // March 1st
            var endOfWhaleSeason = new DateTime(currentYear, 6, 1); // June 1st

            if (coordinate.Latitude > _centreLatitudeInDegrees && coordinate.Longitude > _centreLongitudeInDegrees
                && currentDate >= startOfWhaleSeason && currentDate < endOfWhaleSeason
                && whaleSpeedLimitInKmPerHour < currentSpeedLimit)
                currentSpeedLimit = whaleSpeedLimitInKmPerHour;

            if (double.IsInfinity(currentSpeedLimit))
                return new None();

            return currentSpeedLimit;
        }

        private static double CalculateDistance(Coordinate coordinate1, Coordinate coordinate2)
        {
            // Calculate the distance between two coordinates using the Haversine formula
            double dLat = (coordinate2.Latitude - coordinate1.Latitude) * Math.PI / 180;
            double dLon = (coordinate2.Longitude - coordinate1.Longitude) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(coordinate1.Latitude * Math.PI / 180) * Math.Cos(coordinate2.Latitude * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = earthRadiusInKm * c;
            return distance;
        }

        private static Coordinate GetSensorCoordinate1()
        {
            var currentCoordinate = GetCurrentCoordinate();
            return new Coordinate(currentCoordinate.Latitude + ((_random.NextDouble() - 0.5) * 0.001), currentCoordinate.Longitude + ((_random.NextDouble() - 0.5) * 0.001));
        }

        private static Coordinate GetSensorCoordinate2()
        {
            var currentCoordinate = GetCurrentCoordinate();
            return new Coordinate(currentCoordinate.Latitude + ((_random.NextDouble() - 0.5) * 0.002), currentCoordinate.Longitude + ((_random.NextDouble() - 0.5) * 0.002));
        }

        private static Coordinate GetSensorCoordinate3()
        {
            var currentCoordinate = GetCurrentCoordinate();
            return new Coordinate(currentCoordinate.Latitude + ((_random.NextDouble() - 0.5) * 0.003), currentCoordinate.Longitude + ((_random.NextDouble() - 0.5) * 0.003));
        }

        private static Coordinate GetCurrentCoordinate()
        {
            IAppCache cache = new CachingService();

            var coordinate = cache.GetOrAdd("CurrentCoordinate", () => GetCurrentCoordinateInner(), new MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(200) });

            System.Diagnostics.Debug.WriteLine(coordinate.ToString());

            return coordinate;

            Coordinate GetCurrentCoordinateInner()
            {
                _currentAngleInRadians += 10 * Math.PI / 180;
                var centreLatitudeInRadians = ConvertToRadians(_centreLatitudeInDegrees);
                var cantreLongitudeInRadians = ConvertToRadians(_centreLongitudeInDegrees);
                var circleRadiusToEarthRadiusRatio = circleRadiusInKm / earthRadiusInKm;

                // Calculate new latitude and longitude values based on angle and circle radius using the Haversine formula
                // to determine the coordinates on the surface of a sphere of radius equal to Earth's radius
                var newLatitudeInRadians = Math.Asin(Math.Sin(centreLatitudeInRadians) * Math.Cos(circleRadiusToEarthRadiusRatio) +
                    Math.Cos(centreLatitudeInRadians) * Math.Sin(circleRadiusToEarthRadiusRatio) * Math.Cos(_currentAngleInRadians));

                var newLongitudeInRadians = cantreLongitudeInRadians + Math.Atan2(Math.Sin(_currentAngleInRadians) * Math.Sin(circleRadiusToEarthRadiusRatio) * Math.Cos(centreLatitudeInRadians),
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
}
