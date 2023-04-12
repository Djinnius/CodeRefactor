using DependencyInjectionWithPackages.Options;
using DependencyInjectionWithPackages.Services.CurrentCoordinateProvider;
using Microsoft.Extensions.Options;
using OneOf;
using OneOf.Types;

namespace DependencyInjectionWithPackages.Services.SpeedLimitProviders;

/// <inheritdoc cref="ISpeedLimitProvider"/>
public sealed class EnvironmentalSpeedLimitProvider : ISpeedLimitProvider
{
    private readonly ICurrentCoordinateProvider _currentCoordinateProvider;
    private readonly CircleOptions _circleOptions;
    private readonly SpeedLimitOptions _speedLimitOptions;

    /// <inheritdoc cref="EnvironmentalSpeedLimitProvider"/>
    public EnvironmentalSpeedLimitProvider(
        ICurrentCoordinateProvider currentCoordinateProvider,
        IOptions<CircleOptions> circleOptions,
        IOptions<SpeedLimitOptions> speedLimitOptions)
    {
        _currentCoordinateProvider = currentCoordinateProvider;
        _circleOptions = circleOptions.Value;
        _speedLimitOptions = speedLimitOptions.Value;
    }

    public OneOf<double, None> GetCurrentSpeedLimit()
    {
        var currentCoordinate = _currentCoordinateProvider.GetCurrentCoordinate();

        // Environmental speed restrictions apply in top half of circle
        if (currentCoordinate.Latitude > _circleOptions.CircleCentreCoordinate.Latitude)
            return _speedLimitOptions.EnvironmentalSpeedLimitInKm;

        return new None();
    }
}
