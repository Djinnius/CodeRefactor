﻿using DependencyInjectionWithPackages.Options;
using DependencyInjectionWithPackages.Services.CircleTracker;
using DependencyInjectionWithPackages.Services.CurrentCoordinateProvider;
using Microsoft.Extensions.Options;
using OneOf;
using OneOf.Types;

namespace DependencyInjectionWithPackages.Services.SpeedLimitProviders;

/// <inheritdoc cref="ISpeedLimitProvider"/>
public class PortSpeedLimitProvider : ISpeedLimitProvider
{
    private readonly ICurrentCoordinateProvider _coordinateAggregator;
    private readonly ICircleTracker _circleTracker;
    private readonly SpeedLimitOptions _speedLimitOptions;

    /// <inheritdoc cref="PortSpeedLimitProvider"/>
    public PortSpeedLimitProvider(ICurrentCoordinateProvider coordinateAggregator, ICircleTracker circleTracker, IOptions<SpeedLimitOptions> speedLimitOptions)
    {
        _coordinateAggregator = coordinateAggregator;
        _circleTracker = circleTracker;
        _speedLimitOptions = speedLimitOptions.Value;
    }

    public OneOf<double, None> GetCurrentSpeedLimit()
    {
        var currentCoordinate = _coordinateAggregator.GetCurrentCoordinate();

        // Port speed restrictions apply in right half of circle
        if (currentCoordinate.Longitude > _circleTracker.CircleCentreCoordinate.Longitude)
            return _speedLimitOptions.PortSpeedLimitInKm;

        return new None();
    }
}
