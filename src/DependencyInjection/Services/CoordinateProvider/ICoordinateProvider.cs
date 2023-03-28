using DependencyInjection.Objects;

namespace DependencyInjection.Services.CoordinateProvider;

public interface ICoordinateProvider
{
    Coordinate GetSensorCoordinate1(double currentAngleInRadians, double centreLatitudeInDegrees, double centreLongitudeInDegrees);
    Coordinate GetSensorCoordinate2(double currentAngleInRadians, double centreLatitudeInDegrees, double centreLongitudeInDegrees);
    Coordinate GetSensorCoordinate3(double currentAngleInRadians, double centreLatitudeInDegrees, double centreLongitudeInDegrees);
}
