using GpsPackage.DataObjects;

namespace DependencyInjectionWithPackages.Services.CurrentCoordinateProvider;

/// <summary>
///     Provider service for retrieving the current coordinate.
/// </summary>
public interface ICurrentCoordinateProvider
{
    /// <summary>
    ///     Gets the current mean coordinate excluding outliers.
    /// </summary>
    /// <returns> The current mean <see cref="Coordinate"/> excluding outliers. </returns>
    Coordinate GetCurrentCoordinate();
}
