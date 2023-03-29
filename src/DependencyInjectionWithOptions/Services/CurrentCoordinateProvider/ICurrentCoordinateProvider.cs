using DependencyInjectionWithOptions.Objects;

namespace DependencyInjectionWithOptions.Services.CurrentCoordinateProvider;

/// <summary>
///     Provides the current coordinates.
/// </summary>
public interface ICurrentCoordinateProvider
{
    /// <summary>
    ///     Gets the current mean coordinate excluding outliers.
    /// </summary>
    /// <returns> The current mean <see cref="Coordinate"/> excluding outliers. </returns>
    Coordinate GetCurrentCoordinate();
}
