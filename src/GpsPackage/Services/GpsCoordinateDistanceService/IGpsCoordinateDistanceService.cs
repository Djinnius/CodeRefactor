namespace GpsPackage.Services.GpsCoordinateDistanceService;

/// <summary>
///     Contains service methods for determining distance between two or more coordinates.
/// </summary>
public interface IGpsCoordinateDistanceService
{
    /// <summary>
    ///     Gets the distance between two coordinates on a globe in kilometers.
    /// </summary>
    /// <param name="coordinate1"> The first coordinate. </param>
    /// <param name="coordinate2"> The second coordinate. </param>
    /// <returns> The distance between two coordinates in kilometers. </returns>
    double GetDistanceBetweenTwoCoordinatesInKilometers(Coordinate coordinate1, Coordinate coordinate2);
}
