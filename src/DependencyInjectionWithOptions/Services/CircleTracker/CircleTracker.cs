using DependencyInjectionWithOptions.Objects;
using DependencyInjectionWithOptions.Options;
using Microsoft.Extensions.Options;

namespace DependencyInjectionWithOptions.Services.CircleTracker;

/// <inheritdoc cref="ICircleTracker"/>
public class CircleTracker : ICircleTracker // Singleton
{
    /// <inheritdoc cref="CircleTracker"/>
    public CircleTracker()
    {
        CurrentAngleInRadians = 0; // North
        CircleCentreCoordinate = new Coordinate(51.5167, -0.1246);
    }

    public double CurrentAngleInRadians { get;private set; }

    public Coordinate CircleCentreCoordinate {get; private set; }

    public double IncrementAngle(double incrementInRadians)
    {
        CurrentAngleInRadians += incrementInRadians;
        return CurrentAngleInRadians;
    }
}
