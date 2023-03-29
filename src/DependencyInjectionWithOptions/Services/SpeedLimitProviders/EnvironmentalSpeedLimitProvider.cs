using DependencyInjectionWithOptions.Options;
using DependencyInjectionWithOptions.Services.CircleTracker;
using DependencyInjectionWithOptions.Services.CurrentCoordinateProvider;
using Microsoft.Extensions.Options;
using OneOf;
using OneOf.Types;

namespace DependencyInjectionWithOptions.Services.SpeedLimitProviders;

/// <inheritdoc cref="ISpeedLimitProvider"/>
public class EnvironmentalSpeedLimitProvider : ISpeedLimitProvider
{
    private readonly ICurrentCoordinateProvider _coordinateAggregator;
    private readonly ICircleTracker _circleTracker;
    private readonly SpeedLimitOptions _speedLimitOptions;

    /// <inheritdoc cref="EnvironmentalSpeedLimitProvider"/>
    public EnvironmentalSpeedLimitProvider(ICurrentCoordinateProvider coordinateAggregator, ICircleTracker circleTracker, IOptions<SpeedLimitOptions> speedLimitOptions)
    {
        _coordinateAggregator = coordinateAggregator;
        _circleTracker = circleTracker;
        _speedLimitOptions = speedLimitOptions.Value;
    }

    public OneOf<double, None> GetCurrentSpeedLimit()
    {
        var currentCoordinate = _coordinateAggregator.GetCurrentCoordinate();

        // Environmental speed restrictions apply in top half of circle
        if (currentCoordinate.Latitude > _circleTracker.CircleCentreCoordinate.Latitude)
            return _speedLimitOptions.EnvironmentalSpeedLimitInKm;

        return new None();
    }
}
