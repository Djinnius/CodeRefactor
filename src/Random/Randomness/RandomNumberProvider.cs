using Microsoft.Extensions.Options;
using PseudoRandom.Package.Options;

namespace PseudoRandom.Package.Randomness;

internal sealed class RandomNumberProvider : IRandomNumberProvider
{
    private readonly RandomProviderOptions _randomProviderOptions;
    private readonly Random _random;

    public RandomNumberProvider(IOptions<RandomProviderOptions> randomOptions)
    {
        _randomProviderOptions = randomOptions.Value;

        _random = _randomProviderOptions.Seed is null 
            ? new Random() 
            : new Random(_randomProviderOptions.Seed.Value);
    }

    public int? GetSeed => _randomProviderOptions.Seed;

    public double GetNextDouble => _random.NextDouble();

    public double GetNextDoubleBetween(double firstBound, double secondBound)
    {
        // Check for non-numeric values in minValue
        if (double.IsNaN(firstBound) || double.IsInfinity(firstBound))
            throw new ArgumentException("Invalid minValue: " + firstBound);

        // Check for non-numeric values in maxValue
        if (double.IsNaN(secondBound) || double.IsInfinity(secondBound))
            throw new ArgumentException("Invalid maxValue: " + secondBound);

        var minimum = firstBound < secondBound ? firstBound : secondBound;
        var range = secondBound - firstBound;
        return minimum + _random.NextDouble() * range;
    }
}
