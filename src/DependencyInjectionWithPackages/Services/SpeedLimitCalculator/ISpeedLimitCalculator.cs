using OneOf.Types;
using OneOf;

namespace DependencyInjectionWithPackages.Services.SpeedLimitCalculator;

/// <summary>
///     A utility class for determining speed limits in different contexts.
/// </summary>
public interface ISpeedLimitCalculator
{
    /// <summary>
    ///     Determines the minimum speed limit amongst available speed limits for the current location.
    /// </summary>
    /// <returns> 
    ///     A <see cref="double"/> containing the speed limit in km/hr if a speed limit is applicable, 
    ///     otherwise <see cref="None"/>. 
    /// </returns>
    OneOf<double, None> GetSpeedLimitForCurrentPosition();
}
