using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutOptimizer;
public interface IRandomProvider
{
    public int GenRange(int min, int max);
    public double GenRange(double min, double max);
    public int RandomInt();
    T Choose<T>(List<T> choices);
}
