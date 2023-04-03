using DependencyInjectionWithPackages.Services.SpeedLimitProviders;
using OneOf;
using OneOf.Types;

namespace DependencyInjectionWithPackages.Services.SpeedLimitCalculator;

/// <inheritdoc cref="ISpeedLimitCalculator"/>
public sealed class SpeedLimitCalculator : ISpeedLimitCalculator
{
    private readonly IEnumerable<ISpeedLimitProvider> _speedLimitProviders;

    /// <inheritdoc cref="SpeedLimitCalculator"/>
    public SpeedLimitCalculator(IEnumerable<ISpeedLimitProvider> speedLimitProviders)
    {
        _speedLimitProviders = speedLimitProviders;
    }

    public OneOf<double, None> GetSpeedLimitForCurrentPosition()
    {
        var speedLimits = _speedLimitProviders.Select(x => x.GetCurrentSpeedLimit()).Where(x => x.IsT0).Select(x => x.AsT0).ToList();

        if (speedLimits.Any())
            return speedLimits.MinBy(x => x); // most restrictive.

        return new None();
    }
}
