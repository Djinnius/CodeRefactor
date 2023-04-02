using DependencyInjectionWithPackages.Objects;
using DependencyInjectionWithPackages.Options;
using Microsoft.Extensions.Options;

namespace DependencyInjectionWithPackages.Services.CircleTracker;

/// <inheritdoc cref="ICircleTracker"/>
public class CircleTracker : ICircleTracker // Singleton
{

    /// <inheritdoc cref="CircleTracker"/>
    public CircleTracker(IOptions<CircleOptions> circleOptions)
    {
        CurrentAngleInRadians = 0; // North
        CircleCentreCoordinate = circleOptions.Value.CircleCentreCoordinate;
    }

    public double CurrentAngleInRadians { get;private set; }

    public Coordinate CircleCentreCoordinate {get; private set; }

    public double IncrementAngle(double incrementInRadians)
    {
        CurrentAngleInRadians += incrementInRadians;
        return CurrentAngleInRadians;
    }
}
