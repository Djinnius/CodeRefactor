using DependencyInjectionWithEnumerables.Objects;

namespace DependencyInjectionWithEnumerables.Services.CircleTracker;

/// <inheritdoc cref="ICircleTracker"/>
public class CircleTracker : ICircleTracker // Singleton
{
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
