using GpsPackage.DataObjects;

namespace GpsPackage.Services.GpsCoordinateAveragingService;

/// <summary>
///     Contains service methods for averaging multiple provided coordinates.
/// </summary>
public interface IGpsCoordinateAveragingService
{
    /// <summary>
    ///     Gets the provided coordinates, <paramref name="coordinate1"/>, <paramref name="coordinate2"/>
    ///     and <paramref name="coordinate3"/>, performs error checking to remove outliers, and returns
    ///     a new <see cref="Coordinate"/> containing the average Latitude and average Longitude of the 
    ///     remaining valid coordinates.
    /// </summary>
    /// <param name="coordinate1"> The first coordinate. </param>
    /// <param name="coordinate2"> The second coordinate. </param>
    /// <param name="coordinate3"> The third coordinate. </param>
    /// <returns> 
    ///     A new <see cref="Coordinate"/> containing the average Latitude and Longitude of the provided 
    ///     coordinates, excluding outliers. 
    /// </returns>
    Coordinate GetAverageCoordinateWithErrorCorrection(Coordinate coordinate1, Coordinate coordinate2, Coordinate coordinate3);
}
