using OneOf.Types;
using OneOf;

namespace DependencyInjectionWithOptions.Services.SpeedLimitProviders;

/// <summary>
///     A class for providing speed limits under specific conditions.
/// </summary>
public interface ISpeedLimitProvider
{
    /// <summary>
    ///     Method to retrieve the current speed limit in km/hr if applicable.
    /// </summary>
    /// <returns> A <see cref="double"/> containing a numeric value if a speed limit applies, otherwise <see cref="None"/>. </returns>
    OneOf<double, None> GetCurrentSpeedLimit();
}
