using GpsPackage.DataObjects;

namespace DependencyInjectionWithPackages.Services.CircleTracker;

/// <summary>
///     Tracker for circle properties that may be updated regularly through the lifetime of the application.
/// </summary>
public interface ICircleTracker
{
    /// <summary>
    ///     The current angle within the circle from a reference of 0 being North.
    /// </summary>
    double CurrentAngleInRadians { get; }

    /// <summary>
    ///     Increases the <see cref="CurrentAngleInRadians"/> by the specified <paramref name="incrementInRadians"/>.
    /// </summary>
    /// <param name="incrementInRadians"> The number of radians to increment angle of the circle by. </param>
    /// <returns> The new current angle. </returns>
    double IncrementAngle(double incrementInRadians);
}
