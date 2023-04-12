namespace DependencyInjectionWithPackages.Services.CircleTracker;

/// <inheritdoc cref="ICircleTracker"/>
public sealed class CircleTracker : ICircleTracker // Singleton
{
    /// <inheritdoc cref="CircleTracker"/>
    public CircleTracker()
    {
        CurrentAngleInRadians = 0; // North
    }

    public double CurrentAngleInRadians { get; private set; }

    public double IncrementAngle(double incrementInRadians)
    {
        CurrentAngleInRadians += incrementInRadians;
        return CurrentAngleInRadians;
    }
}
