using DependencyInjectionWithEnumerables.Objects;

namespace DependencyInjectionWithEnumerables.Services.CurrentCoordinateAggregator;

/// <summary>
///     An aggregator taking multiple current coordinator inputs and returning
///     a mean current coordinate with error checking.
/// </summary>
public interface ICurrentCoordinateAggregator
{
    /// <summary>
    ///     Gets the current mean coordinate excluding outliers.
    /// </summary>
    /// <returns> The current mean <see cref="Coordinate"/> excluding outliers. </returns>
    Coordinate GetCurrentCoordinate();
}
