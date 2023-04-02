using DependencyInjectionWithEnumerables.Services.CircleTracker;
using DependencyInjectionWithEnumerables.Services.CurrentCoordinateAggregator;
using OneOf;
using OneOf.Types;

namespace DependencyInjectionWithEnumerables.Services.SpeedLimitProviders;

/// <inheritdoc cref="ISpeedLimitProvider"/>
public class EnvironmentalSpeedLimitProvider : ISpeedLimitProvider
{
    private readonly ICurrentCoordinateAggregator _coordinateAggregator;
    private readonly ICircleTracker _circleTracker;
    private static double environmentalSpeedLimitInKmPerHour = 30; // navigate efficiently

    /// <inheritdoc cref="EnvironmentalSpeedLimitProvider"/>
    public EnvironmentalSpeedLimitProvider(ICurrentCoordinateAggregator coordinateAggregator, ICircleTracker circleTracker)
    {
        _coordinateAggregator = coordinateAggregator;
        _circleTracker = circleTracker;
    }

    public OneOf<double, None> GetCurrentSpeedLimit()
    {
        var currentCoordinate = _coordinateAggregator.GetCurrentCoordinate();

        // Environmental speed restrictions apply in top half of circle
        if (currentCoordinate.Latitude > _circleTracker.CircleCentreCoordinate.Latitude)
            return environmentalSpeedLimitInKmPerHour;

        return new None();
    }
}
