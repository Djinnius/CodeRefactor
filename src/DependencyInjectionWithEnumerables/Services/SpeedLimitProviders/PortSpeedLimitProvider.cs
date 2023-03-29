using DependencyInjectionWithOptions.Services.CircleTracker;
using DependencyInjectionWithOptions.Services.CurrentCoordinateAggregator;
using OneOf;
using OneOf.Types;

namespace DependencyInjectionWithOptions.Services.SpeedLimitProviders;

/// <inheritdoc cref="ISpeedLimitProvider"/>
public class PortSpeedLimitProvider : ISpeedLimitProvider
{
    private readonly ICurrentCoordinateAggregator _coordinateAggregator;
    private readonly ICircleTracker _circleTracker;
    private readonly double portSpeedLimitInKmPerHour = 20; // navigate carefully in busy places

    /// <inheritdoc cref="PortSpeedLimitProvider"/>
    public PortSpeedLimitProvider(ICurrentCoordinateAggregator coordinateAggregator, ICircleTracker circleTracker)
    {
        _coordinateAggregator = coordinateAggregator;
        _circleTracker = circleTracker;
    }

    public OneOf<double, None> GetCurrentSpeedLimit()
    {
        var currentCoordinate = _coordinateAggregator.GetCurrentCoordinate();

        // Port speed restrictions apply in right half of circle
        if (currentCoordinate.Longitude > _circleTracker.CircleCentreCoordinate.Longitude)
            return portSpeedLimitInKmPerHour;

        return new None();
    }
}
