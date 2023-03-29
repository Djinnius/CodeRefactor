using DependencyInjectionWithOptions.Objects;

namespace DependencyInjectionWithOptions.Services.CurrentCoordinateProvider;

/// <summary>
///     A provider of current coordinates.
/// </summary>
public interface ICurrentCoordinateProvider
{
    /// <summary>
    ///     Gets the 1st sensor gps coordinate.
    /// </summary>
    /// <returns> A coordinate containing the current location of sensor 1. </returns>
    Coordinate GetSensorCoordinate1();

    /// <summary>
    ///     Gets the 2nd sensor gps coordinate.
    /// </summary>
    /// <returns> A coordinate containing the current location of sensor 2. </returns>
    Coordinate GetSensorCoordinate2();

    /// <summary>
    ///     Gets the 3rd sensor gps coordinate.
    /// </summary>
    /// <returns> A coordinate containing the current location of sensor 3. </returns>
    Coordinate GetSensorCoordinate3();
}
