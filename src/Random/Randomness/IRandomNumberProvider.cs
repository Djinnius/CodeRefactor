namespace PseudoRandom.Package.Randomness;

/// <summary>
///     Provides random numbers.
/// </summary>
public interface IRandomNumberProvider
{
    /// <summary>
    ///     Returns the current seed used within the random provider if set, otherwise <see langword="null"/>.
    /// </summary>
    int? GetSeed { get; }

    /// <inheritdoc cref="Random.NextDouble"/>
    double GetNextDouble {  get; }

    /// <summary>
    ///     Returns a random floating-point number that is greater than or equal to the minimum of the <paramref name="firstBound"/>
    ///     and <paramref name="secondBound"/>, and less than the greater of the two.
    /// </summary>
    /// <param name="firstBound"> The first boundary. </param>
    /// <param name="secondBound"> The second boundary. </param>
    /// <returns> 
    ///     A random floating-point number that is greater than or equal to the minimum of the <paramref name="firstBound"/>
    ///     and <paramref name="secondBound"/>, and less than the greater of the two.
    /// </returns>
    double GetNextDoubleBetween(double firstBound, double secondBound);
}
