using GpsPackage.DataObjects;

namespace GpsPackage.Extensions;

/// <summary>
///     Extension methods for the <see cref="Coordinate"/> struct.
/// </summary>
public static class CoordinateExtensions
{
    /// <summary>
    ///     A <see cref="Coordinate"/> extension method that takes the coordinates <paramref name="coordinate1"/>,
    ///     <paramref name="coordinate2"/>, and returns an average coordinate position.
    /// </summary>
    /// <param name="coordinate1"> The first coordinate. </param>
    /// <param name="coordinate2"> The second coordinate. </param>
    /// <returns> A new <see cref="Coordinate"/> containing the average Latitude and Longitude of all provided coordinates. </returns>
    public static Coordinate AverageWith(this Coordinate coordinate1, Coordinate coordinate2)
        => new((coordinate1.Latitude + coordinate2.Latitude) / 2, (coordinate1.Longitude + coordinate2.Longitude) / 2);

    /// <summary>
    ///     A <see cref="Coordinate"/> extension method that takes the coordinates <paramref name="coordinate1"/>,
    ///     <paramref name="coordinate2"/> and <paramref name="coordinate3"/>, and returns an average coordinate 
    ///     position.
    /// </summary>
    /// <param name="coordinate1"> The first coordinate. </param>
    /// <param name="coordinate2"> The second coordinate. </param>
    /// <param name="coordinate3"> The third coordinate. </param>
    /// <returns> A new <see cref="Coordinate"/> containing the average Latitude and Longitude of all provided coordinates. </returns>
    public static Coordinate AverageWith(this Coordinate coordinate1, Coordinate coordinate2, Coordinate coordinate3)
        => new((coordinate1.Latitude + coordinate2.Latitude + coordinate3.Latitude) / 3, (coordinate1.Longitude + coordinate2.Longitude + coordinate3.Longitude) / 3);

    /// <summary>
    ///     A <see cref="Coordinate"/> extension method that takes the <paramref name="coordinate"/>
    ///     and a collection of provide <paramref name="coordinates"/>, and returns an average coordinate 
    ///     position.
    /// </summary>
    /// <param name="coordinate"> The source coordinate. </param>
    /// <param name="coordinates"> A variable length parameters list of coordinates to average the <paramref name="coordinate"/> with. </param>
    /// <returns> A new <see cref="Coordinate"/> containing the average Latitude and Longitude of all provided coordinates. </returns>
    public static Coordinate AverageWith(this Coordinate coordinate, params Coordinate[] coordinates)
        => new Coordinate(
            coordinates.Concat(new List<Coordinate> { coordinate }).AverageLatitude(),
            coordinates.Concat(new List<Coordinate> { coordinate }).AverageLongitude());

    /// <summary>
    ///     A <see cref="Coordinate"/> extension method that takes the <paramref name="coordinate"/>
    ///     and a collection of provide <paramref name="coordinates"/>, and returns an average coordinate 
    ///     position.
    /// </summary>
    /// <param name="coordinate"> The source coordinate. </param>
    /// <param name="coordinates"> The collection of coordinates to average the <paramref name="coordinate"/> with. </param>
    /// <returns> A new <see cref="Coordinate"/> containing the average Latitude and Longitude of all provided coordinates. </returns>
    public static Coordinate AverageWith(this Coordinate coordinate, IEnumerable<Coordinate> coordinates)
        => new Coordinate(
            coordinates.Concat(new List<Coordinate> { coordinate }).AverageLatitude(),
            coordinates.Concat(new List<Coordinate> { coordinate }).AverageLongitude());

    private static double AverageLatitude(this IEnumerable<Coordinate> coordinates)
        => coordinates.Select(x => x.Latitude).Average();

    private static double AverageLongitude(this IEnumerable<Coordinate> coordinates)
        => coordinates.Select(x => x.Latitude).Average();
}
