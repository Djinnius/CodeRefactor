using DependencyInjectionWithPackages.Options;
using DependencyInjectionWithPackages.Services.CurrentCoordinateProvider;
using Microsoft.Extensions.Options;
using OneOf;
using OneOf.Types;

namespace DependencyInjectionWithPackages.Services.SpeedLimitProviders;

/// <inheritdoc cref="ISpeedLimitProvider"/>
public sealed class PortSpeedLimitProvider : ISpeedLimitProvider
{
    private readonly ICurrentCoordinateProvider _coordinateAggregator;
    private readonly CircleOptions _circleOptions;
    private readonly SpeedLimitOptions _speedLimitOptions;

    /// <inheritdoc cref="PortSpeedLimitProvider"/>
    public PortSpeedLimitProvider(
        ICurrentCoordinateProvider coordinateAggregator,
        IOptions<CircleOptions> circleOptions,
        IOptions<SpeedLimitOptions> speedLimitOptions)
    {
        _coordinateAggregator = coordinateAggregator;
        _circleOptions = circleOptions.Value;
        _speedLimitOptions = speedLimitOptions.Value;
    }

    public OneOf<double, None> GetCurrentSpeedLimit()
    {
        var currentCoordinate = _coordinateAggregator.GetCurrentCoordinate();

        // Port speed restrictions apply in right half of circle
        if (currentCoordinate.Longitude > _circleOptions.CircleCentreCoordinate.Longitude)
            return _speedLimitOptions.PortSpeedLimitInKm;

        return new None();
    }
}
