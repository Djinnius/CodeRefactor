using DependencyInjectionWithPackages.Options;
using DependencyInjectionWithPackages.Services.CurrentCoordinateProvider;
using Microsoft.Extensions.Options;
using OneOf;
using OneOf.Types;
using Timing.Package.Clocks;

namespace DependencyInjectionWithPackages.Services.SpeedLimitProviders;

/// <inheritdoc cref="ISpeedLimitProvider"/>
public sealed class WhaleSpeedLimitProvider : ISpeedLimitProvider
{
    private readonly ICurrentCoordinateProvider _coordinateAggregator;
    private readonly IClock _clock;
    private readonly CircleOptions _circleOptions;
    private readonly SpeedLimitOptions _speedLimitOptions;

    /// <inheritdoc cref="WhaleSpeedLimitProvider"/>
    public WhaleSpeedLimitProvider(
        ICurrentCoordinateProvider coordinateAggregator,
        IClock clock,
        IOptions<CircleOptions> circleOptions,
        IOptions<SpeedLimitOptions> speedLimitOptions)
    {
        _coordinateAggregator = coordinateAggregator;
        _clock = clock;
        _circleOptions = circleOptions.Value;
        _speedLimitOptions = speedLimitOptions.Value;
    }

    public OneOf<double, None> GetCurrentSpeedLimit()
    {
        var currentCoordinate = _coordinateAggregator.GetCurrentCoordinate();

        var currentDate = _clock.Now.Date;
        var currentYear = currentDate.Year;
        var startOfWhaleSeason = new DateTime(currentYear, 3, 1); // March 1st
        var endOfWhaleSeason = new DateTime(currentYear, 6, 1); // June 1st

        // Whale speed restrictions apply in top right quadrant of circle in March, April and May
        if (currentCoordinate.Latitude > _circleOptions.CircleCentreCoordinate.Latitude &&
            currentCoordinate.Longitude > _circleOptions.CircleCentreCoordinate.Longitude &&
            currentDate >= startOfWhaleSeason && currentDate < endOfWhaleSeason)
            return _speedLimitOptions.WhaleSpeedLimitInKm;

        return new None();
    }
}
