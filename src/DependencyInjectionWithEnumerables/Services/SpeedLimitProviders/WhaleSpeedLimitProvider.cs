using DependencyInjectionWithEnumerables.Providers.Clock;
using DependencyInjectionWithEnumerables.Services.CircleTracker;
using DependencyInjectionWithEnumerables.Services.CurrentCoordinateAggregator;
using OneOf;
using OneOf.Types;

namespace DependencyInjectionWithEnumerables.Services.SpeedLimitProviders;

/// <inheritdoc cref="ISpeedLimitProvider"/>
public class WhaleSpeedLimitProvider : ISpeedLimitProvider
{
    private readonly ICurrentCoordinateAggregator _coordinateAggregator;
    private readonly ICircleTracker _circleTracker;
    private readonly IClock _clock;
    private static double whaleSpeedLimitInKmPerHour = 15; // don't disturb whales

    /// <inheritdoc cref="WhaleSpeedLimitProvider"/>
    public WhaleSpeedLimitProvider(ICurrentCoordinateAggregator coordinateAggregator, ICircleTracker circleTracker, IClock clock)
    {
        _coordinateAggregator = coordinateAggregator;
        _circleTracker = circleTracker;
        _clock = clock;
    }

    public OneOf<double, None> GetCurrentSpeedLimit()
    {
        var currentCoordinate = _coordinateAggregator.GetCurrentCoordinate();

        var currentDate = _clock.Now.Date;
        var currentYear = currentDate.Year;
        var startOfWhaleSeason = new DateTime(currentYear, 3, 1); // March 1st
        var endOfWhaleSeason = new DateTime(currentYear, 6, 1); // June 1st

        // Whale speed restrictions apply in top right quadrant of circle in March, April and May
        if (currentCoordinate.Latitude > _circleTracker.CircleCentreCoordinate.Latitude && 
            currentCoordinate.Longitude > _circleTracker.CircleCentreCoordinate.Longitude && 
            currentDate >= startOfWhaleSeason && currentDate < endOfWhaleSeason)
            return whaleSpeedLimitInKmPerHour;

        return new None();
    }
}
