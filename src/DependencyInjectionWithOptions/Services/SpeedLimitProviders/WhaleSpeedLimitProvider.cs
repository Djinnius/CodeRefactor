﻿using DependencyInjectionWithOptions.Options;
using DependencyInjectionWithOptions.Providers.Clock;
using DependencyInjectionWithOptions.Services.CircleTracker;
using DependencyInjectionWithOptions.Services.CurrentCoordinateProvider;
using Microsoft.Extensions.Options;
using OneOf;
using OneOf.Types;

namespace DependencyInjectionWithOptions.Services.SpeedLimitProviders;

/// <inheritdoc cref="ISpeedLimitProvider"/>
public class WhaleSpeedLimitProvider : ISpeedLimitProvider
{
    private readonly ICurrentCoordinateProvider _coordinateAggregator;
    private readonly ICircleTracker _circleTracker;
    private readonly IClock _clock;
    private readonly SpeedLimitOptions _speedLimitOptions;

    /// <inheritdoc cref="WhaleSpeedLimitProvider"/>
    public WhaleSpeedLimitProvider(ICurrentCoordinateProvider coordinateAggregator, ICircleTracker circleTracker, IClock clock, IOptions<SpeedLimitOptions> speedLimitOptions)
    {
        _coordinateAggregator = coordinateAggregator;
        _circleTracker = circleTracker;
        _clock = clock;
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
        if (currentCoordinate.Latitude > _circleTracker.CircleCentreCoordinate.Latitude && 
            currentCoordinate.Longitude > _circleTracker.CircleCentreCoordinate.Longitude && 
            currentDate >= startOfWhaleSeason && currentDate < endOfWhaleSeason)
            return _speedLimitOptions.WhaleSpeedLimitInKm;

        return new None();
    }
}
