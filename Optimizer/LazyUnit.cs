using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutOptimizer;
public class LazyUnit<T> where T : IUnit<T>
{
    public T Unit { get; private set; }
    private double? lazyFitness;

    public double Fitness
    { 
        get
        {
            if (!lazyFitness.HasValue)
                lazyFitness = Unit.Fitness;
            return lazyFitness.Value;
        }
    }

    public LazyUnit(T unit)
    {
        Unit = unit;
    }
    public static implicit operator LazyUnit<T>(T unit)
    {
        return new LazyUnit<T>(unit);
    }
}
