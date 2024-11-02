using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutOptimizer;
public interface IUnit : IUnit<IUnit>
{ 

}
public interface IUnit<TSelf> where TSelf : IUnit<TSelf>
{
    public double Fitness { get; }
    public TSelf BreedWith(IUnit<TSelf> other, IRandomProvider rng);
}
