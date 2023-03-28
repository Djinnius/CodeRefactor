using DependencyInjection.Objects;

namespace DependencyInjection.Services.CoordinateAggregator;

public interface ICoordinateAggregator
{
    /// <summary>
    ///     Gets the current mean coordinate excluding outliers.
    /// </summary>
    /// <param name="_currentAngleInRadians"> The angle around a circle. </param>
    /// <param name="centreLatitudeInDegrees"> The latitude of the centre point of a circle. </param>
    /// <param name="centreLongitudeInDegrees"> The longitude of the centre point of a circle. </param>
    /// <returns></returns>
    Coordinate GetCurrentCoordinate(double _currentAngleInRadians, double centreLatitudeInDegrees, double centreLongitudeInDegrees);
}
