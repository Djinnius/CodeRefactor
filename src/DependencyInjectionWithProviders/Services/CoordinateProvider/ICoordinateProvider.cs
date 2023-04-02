using DependencyInjectionWithProviders.Objects;

namespace DependencyInjectionWithProviders.Services.CoordinateProvider;

public interface ICoordinateProvider
{
    Coordinate GetSensorCoordinate1(double currentAngleInRadians, double centreLatitudeInDegrees, double centreLongitudeInDegrees);
    Coordinate GetSensorCoordinate2(double currentAngleInRadians, double centreLatitudeInDegrees, double centreLongitudeInDegrees);
    Coordinate GetSensorCoordinate3(double currentAngleInRadians, double centreLatitudeInDegrees, double centreLongitudeInDegrees);
}
