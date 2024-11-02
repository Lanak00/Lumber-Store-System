
namespace CutOptimizer;

internal class SeedableRng : IRandomProvider
{
    public ulong Seed { get; }

    Random rng;

    public SeedableRng(ulong seed)
    {
        Seed = seed;
        rng = new Random((int)seed);
    }


    public int GenRange(int min, int max)
    {
        return rng.Next(min, max);
    }

    public int RandomInt() => rng.Next();

    public T Choose<T>(List<T> choices)
    {
        return choices[rng.Next(0, choices.Count)];
    }

    public double GenRange(double min, double max)
    {
        return rng.NextDouble() * (max - min) + min;
    }
}