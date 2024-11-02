using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutOptimizer;

class LazyUnitSorter<T> : IComparer<LazyUnit<T>> where T : IUnit<T>
{
    public int Compare(LazyUnit<T>? x, LazyUnit<T>? y)
    {
        return y?.Fitness.CompareTo(x?.Fitness) ?? 0;
    }
}

public class Population<T> where T : IUnit<T>
{
    public List<T> Units { get; set; }
    public ulong Seed { get; set; }

    private double breedFactor;
    public double BreedFactor
    {
        get => breedFactor;
        set
        {
            Debug.Assert(value > 0.0 && value <= 1.0);
            breedFactor = value;
        }
    }

    // TODO: Revisit this
    private double survivalFactor;
    public double SurvivalFactor
    {
        get => survivalFactor;
        set
        {
            Debug.Assert(value >= 0.0 && value <= 1.0);
            survivalFactor = value;
        }
    }

    private int maxSize;
    public int MaxSize
    {
        get => maxSize;
        set
        {
            Units = Units.GetRange(0, Math.Min(value, Units.Count));
            maxSize = value;
        }
    }

    public Population(List<T> pop)
    {
        Units = pop;
        Seed = 1;
        BreedFactor = 0.5;
        SurvivalFactor = 0.5;
        MaxSize = 100;
    }

    public IRandomProvider Epoch(List<LazyUnit<T>> units, IRandomProvider rng)
    {
        Debug.Assert(units.Count != 0);
        var breedUpTo = (int)(BreedFactor * units.Count);
        var breeders = units.Take(breedUpTo).ToList();
        units.Clear();

        var survivingParents = (int)Math.Ceiling(SurvivalFactor * breeders.Count());
        for(int i = 0; i < MaxSize - survivingParents; i++)
        {
            var rs = rng.GenRange(0, breeders.Count);
            units.Add(breeders[i % breeders.Count].Unit.BreedWith(breeders[rs].Unit, rng));
        }
        units.AddRange(breeders.Take(survivingParents));
        return rng;
    }

    public Population<T> Epochs(int nEpocs, Action<double> progressCallback)
    {
        var activeStack = new List<LazyUnit<T>>();

        while(Units.Count > 0)
        {
            activeStack.Add(Units[0]);
            Units.RemoveAt(0);
        }
        IRandomProvider rng = new SeedableRng(Seed);

        for (int i = 0; i < nEpocs; i++)
        {
            activeStack.Sort(new LazyUnitSorter<T>());
            if(activeStack.Last()?.Fitness >= 1.0)
            {
                break;
            }

            if(i != nEpocs)
            {
                rng = Epoch(activeStack, rng);
            }

            progressCallback((double)i / nEpocs);
        }

        while (activeStack.Count > 0)
        {
            Units.Add(activeStack[0].Unit);
            activeStack.RemoveAt(0);
        }

        return this;
    }

    public List<T> Finish()
    {
        var emptyUnits = new List<T>();
        emptyUnits.AddRange(Units);
        Units.Clear();
        return emptyUnits;
    }
}
