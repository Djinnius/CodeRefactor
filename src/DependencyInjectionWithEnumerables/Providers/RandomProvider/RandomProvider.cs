namespace DependencyInjectionWithEnumerables.Providers.RandomProvider;

public class RandomProvider : IRandomProvider
{
    private static readonly Random _random = new Random();

    public Random Random => _random;
}
