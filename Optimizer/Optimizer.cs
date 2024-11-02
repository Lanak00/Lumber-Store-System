using System.Runtime.InteropServices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CutOptimizer;

public enum PatternDirection
{
    None,
    ParallelToWidth,
    ParallelToLength
}


public enum Fit
{
    None,
    UprightExact,
    RotatedExact,
    Upright,
    Rotated,
}

public class UsedCutPiece
{
    public int Id { get; set; }
    public int? ExternalId { get; set; }
    public Rect Rect { get; set; }
    public PatternDirection PatternDirection { get; set; }
    public bool IsRotated { get; set; }
    public bool CanRotate { get; set; }

    public override int GetHashCode() => Id.GetHashCode();
    public static bool operator ==(UsedCutPiece x, UsedCutPiece y) => x.Id == y.Id;
    public static bool operator !=(UsedCutPiece x, UsedCutPiece y) => x.Id != y.Id;

    public override bool Equals(object? obj)
    {
        if (obj is UsedCutPiece other)
            return this == other;
        return false;
    }
}


public struct CutPiece
{
    public int Quantity { get; set; }
    public int? ExternalId { get; set; }
    public int Width { get; set; }
    public int Length { get; set; }
    public PatternDirection PatternDirection { get; set; }
    public bool CanRotate { get; set; }
}

public struct ResultCutPiece
{
    public int? ExternalId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Length { get; set; }
    public PatternDirection PatternDirection { get; set; }
    public bool IsRotated { get; set; }

    public static bool operator==(ResultCutPiece x, ResultCutPiece y)
    {
        return x.ExternalId == y.ExternalId &&
               x.X == y.X &&
               x.Y == y.Y &&
               x.Width == y.Width &&
               x.Length == y.Length &&
               x.PatternDirection == y.PatternDirection &&
               x.IsRotated == y.IsRotated;
    }

    public static bool operator!=(ResultCutPiece x, ResultCutPiece y) => !(x == y);

    public static implicit operator ResultCutPiece(UsedCutPiece usedCutPiece)
    {
        return new ResultCutPiece()
        {
            ExternalId = usedCutPiece.ExternalId,
            X = usedCutPiece.Rect.X,
            Y = usedCutPiece.Rect.Y,
            Width = usedCutPiece.Rect.Width,
            Length = usedCutPiece.Rect.Length,
            PatternDirection = usedCutPiece.PatternDirection,
            IsRotated = usedCutPiece.IsRotated
        };
    }

    public readonly override bool Equals(object? obj)
    {
        if(obj is ResultCutPiece other)
            return this == other;
        return false;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(ExternalId, X, Y, Width, Length, PatternDirection, IsRotated);
    }
}

public struct CutPieceWithId
{
    public int Id { get; set; }
    public int? ExternalId { get; set; }
    public int Width { get; set; }
    public int Length { get; set; }
    public PatternDirection PatternDirection { get; set; }
    public bool CanRotate { get; set; }

    public override int GetHashCode() => Id.GetHashCode();
    public static bool operator ==(CutPieceWithId x, CutPieceWithId y) => x.Id == y.Id;
    public static bool operator !=(CutPieceWithId x, CutPieceWithId y) => x.Id != y.Id;

    public static implicit operator CutPieceWithId(UsedCutPiece usedCutPiece)
    {
        var piece = new CutPieceWithId();
        var (width, length, direction) = usedCutPiece.IsRotated switch
        {
            true => (usedCutPiece.Rect.Length, usedCutPiece.Rect.Width, usedCutPiece.PatternDirection.Rotated()),
            false => (usedCutPiece.Rect.Width, usedCutPiece.Rect.Length, usedCutPiece.PatternDirection)
        };
        piece.Id = usedCutPiece.Id;
        piece.ExternalId = usedCutPiece.ExternalId;
        piece.Width = width;
        piece.Length = length;
        piece.CanRotate = usedCutPiece.CanRotate;
        piece.PatternDirection = direction;
        return piece;
    }

    public override bool Equals(object? obj)
    {
        if (obj is CutPieceWithId other)
            return this == other;
        return false;
    }
}

public struct StockPiece
{
    public int Width { get; set; }
    public int Length { get; set; }
    public PatternDirection PatternDirection { get; set; }
    public int Price { get; set; }
    public int? Quantity { get; set; }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(Width, Length, PatternDirection, Price, Quantity);
    }

    public static bool operator ==(StockPiece x, StockPiece y)
    {
        return x.Width == y.Width &&
               x.Length == y.Length &&
               x.PatternDirection == y.PatternDirection &&
               x.Price == y.Price &&
               x.Quantity == y.Quantity;
    }
    public static bool operator !=(StockPiece x, StockPiece y) => !(x == y);

    public bool FitsCutPiece(CutPieceWithId cutPiece)
    {
        var rect = new Rect(0, 0, Width, Length);
        return rect.FitCutPiece(PatternDirection, cutPiece, false) != Fit.None;
    }

    public StockPiece DecQuantity()
    {
        if(Quantity.HasValue)
            Quantity--;
        return this;
    }

    public StockPiece IncQuantity()
    {
        if(Quantity.HasValue)
            Quantity++;
        return this;
    }

    public readonly override bool Equals(object? obj)
    {
        if (obj is StockPiece other)
            return this == other;
        return false;
    }
}

public struct ResultStockPiece
{ 
    public int Width { get; set; }
    public int Length { get; set; }
    public PatternDirection PatternDirection { get; set; }
    public List<ResultCutPiece> CutPieces { get; set; }
    public List<Rect> WastePieces { get; set; }
    public int Price { get; set; }
}

public struct Rect
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Length { get; set; }

    public Rect(int x, int y, int width, int length)
    {
        X = x;
        Y = y;
        Width = width;
        Length = length;
    }

    internal Fit FitCutPiece(PatternDirection patternDirection, CutPieceWithId cutPiece, bool preferRotated)
    {
        Fit? uprightFit;
        Fit? rotatedFit;
        if (cutPiece.PatternDirection == patternDirection)
        {
            if (cutPiece.Width == Width && cutPiece.Length == Length)
                uprightFit = Fit.UprightExact;
            else if (cutPiece.Width <= Width && cutPiece.Length <= Length)
                uprightFit = Fit.Upright;
            else
                uprightFit = null;
        }
        else
        {
            uprightFit = null;
        }

        if (cutPiece.CanRotate && cutPiece.PatternDirection.Rotated() == patternDirection)
        {
            if (cutPiece.Length == Width && cutPiece.Width == Length)
                rotatedFit = Fit.RotatedExact;
            else if (cutPiece.Length <= Width && cutPiece.Width <= Length)
                rotatedFit = Fit.Rotated;
            else
                rotatedFit = null;
        }
        else
        {
            rotatedFit = null;
        }

        return (uprightFit, rotatedFit) switch
        {
            (not null, not null) => preferRotated ? rotatedFit.Value : uprightFit.Value,
            (not null, null) => uprightFit.Value,
            (null, not null) => rotatedFit.Value,
            (null, null) => Fit.None,
        };
    }

    public bool Contains(Rect rect)
    {
        return rect.X >= X &&
            rect.X + rect.Width <= X + Width &&
            rect.Y >= Y &&
            rect.Y + rect.Length <= Y + Length;
    }

    public static implicit operator Rect(ResultCutPiece cutPiece)
    {
        return new Rect(cutPiece.X, cutPiece.Y, cutPiece.Width, cutPiece.Length);
    }
}

internal static class GeneralUtils
{
    public static PatternDirection Rotated(this PatternDirection direction)
    {
        return direction switch
        {
            PatternDirection.None => PatternDirection.None,
            PatternDirection.ParallelToWidth => PatternDirection.ParallelToLength,
            PatternDirection.ParallelToLength => PatternDirection.ParallelToWidth,
            _ => PatternDirection.None
        };
    }

    public static bool IsFitNone(this Fit fit) => fit == Fit.None;
    public static bool IsFitUpright(this Fit fit) => fit == Fit.Upright || fit == Fit.UprightExact;
    public static bool IsFitRotated(this Fit fit) => fit == Fit.Rotated || fit == Fit.RotatedExact;

    public static T? FirstOrNull<T>(this IEnumerable<T> source, Func<T, bool> predicate) where T : struct
    {
        foreach (var item in source)
        {
            if (predicate(item))
                return item;
        }
        return null;
    }

    public static bool ApplyToFirstStruct<T>(this List<T> source, Func<T, bool> predicate, Func<T, T> action) where T : struct
    {
        for(int i = 0; i < source.Count; i++)
        {
            if (predicate(source[i]))
            {
                source[i] = action(source[i]);
                return true;
            }
        }
        return false;
    }

    public static bool TryFirstRef<T>(this List<T> source, Func<T, bool> predicate, ref T result) where T : struct
    {
        for(int i = 0 ; i < source.Count; i++)
        {
            if(predicate(source[i]))
            {
                var span = CollectionsMarshal.AsSpan(source);
                result =  ref span[i];
                return true;
            }
        }
        return false;
    }

    public static int Map(int val, int minA, int maxA, int minB, int maxB)
    {
        return (int)((val - minA) * (maxB - minB) / (maxA - minA) + minB);
    }

    public static void SwapRemoveAt<T>(this List<T> source, int index)
    {
        source[index] = source.Last();
        source.RemoveAt(source.Count - 1);
    }
}


public interface IHeuristic
{ }

public interface IBin
{   
    public static abstract IBin Create(int width, int length, int bladeWidth, PatternDirection patternDirection, int price);
    public ResultStockPiece IntoResultStockPiece();
    public double Fitness();
    public int Price();
    public int RemoveCutPieces<TCut>(IEnumerable<TCut> cutPieces) where TCut : UsedCutPiece;
    public IEnumerable<UsedCutPiece> CutPieces();
    public static abstract List<IHeuristic> PossibleHeuristics();
    public bool InsertCutPieceWithHeuristic(CutPieceWithId cutPieceWithId, IHeuristic heuristic);
    public bool InsertCutPieceRandomHeuristic(CutPieceWithId cutPiece, IRandomProvider rng);
    public bool MatchesStockPiece(StockPiece stockPiece);
}

internal class OptimizerUnit<TBin> : IUnit<OptimizerUnit<TBin>> where TBin : IBin
{
    public List<TBin> Bins { get; set; } = new List<TBin>();
    public ArraySegment<StockPiece> PossibleStockPieces { get; set; }
    public List<StockPiece> AvailableStockPieces { get; set; } = new List<StockPiece>();
    public HashSet<CutPieceWithId> UnusedCutPieces { get; set; } = new HashSet<CutPieceWithId>();
    public int BladeWidth => 0;

    public double Fitness
    {
        get
        {
            var fitness = 0.0;
            if (Bins.Count > 0)
            {
                fitness = Bins.Aggregate(0.0, (acc, b) => acc + b.Fitness()) / Bins.Count;
            }

            if (UnusedCutPieces.Count == 0)

            {
                return fitness;
            }
            return fitness - 1;
        }
    }

    public static OptimizerUnit<TBin> WithRandomHeuristics(
        ArraySegment<StockPiece> possibleStockPieces,
        ArraySegment<CutPieceWithId> cutPieces,
        int bladeWidth,
        IRandomProvider rng)
    {
        var unit = new OptimizerUnit<TBin>
        {
            PossibleStockPieces = possibleStockPieces,
            AvailableStockPieces = possibleStockPieces.ToList(),
        };
        foreach(var cutPiece in cutPieces)
        {
            if (!unit.FirstFitRandomHeuristics(cutPiece, rng))
            {
                unit.UnusedCutPieces.Add(cutPiece);
            }
        }

        return unit;
    }

    public static OptimizerUnit<TBin> WithHeuristic(
        ArraySegment<StockPiece> possibleStockPieces,
        ArraySegment<CutPieceWithId> cutPieces,
        int bladeWidth,
        IHeuristic heuristic,
        IRandomProvider rng)
    {
        var unit = new OptimizerUnit<TBin>
        {
            PossibleStockPieces = possibleStockPieces,
            AvailableStockPieces = possibleStockPieces.ToList(),
        };
        foreach (var cutPiece in cutPieces)
        {
            if (!unit.FirstFitWithHeuristics(cutPiece, heuristic, rng))
            {
                unit.UnusedCutPieces.Add(cutPiece);
            }
        }

        return unit;
    }

    public static List<OptimizerUnit<TBin>> GenerateInitialUnits(
        ArraySegment<StockPiece> possibleStockPieces,
        List<CutPieceWithId> cutPieces,
        int bladeWidth,
        ulong randomSeed)
    {
        var set = new HashSet<(int width, int length, bool canRotate, PatternDirection patternDirection)>();
        foreach(var cutPiece in cutPieces)
        {
            set.Add((
                cutPiece.Width,
                cutPiece.Length,
                cutPiece.CanRotate,
                cutPiece.PatternDirection
                ));
        }

        var uniqueCutPieces = set.Count;
        var possibleHeuristics = TBin.PossibleHeuristics();
        int numUnits = 0;
        if (cutPieces.Count < 3)
        {
            numUnits = possibleHeuristics.Count;
        }
        else
        {
            var denom = 1.0;
            if (cutPieces.Count > 1)
            {
                denom = Math.Log10(cutPieces.Count);
            }

            numUnits = (int)Math.Max(possibleHeuristics.Count * 3,
                cutPieces.Count / denom + ((uniqueCutPieces - 1) * 10.0));
        }

        var units = new List<OptimizerUnit<TBin>>(numUnits);
        var rng = new SeedableRng(randomSeed);

        // TOOD: Revisit
        //var cutPiecesArr = cutPieces.OrderByDescending((p) => p.Length).ThenBy(p => p.Width).ToArray();
        var cutPiecesArr = cutPieces.OrderByDescending(p => (p.Length, p.Width)).ToArray();
        foreach(var heuristic in possibleHeuristics)
        {
            units.Add(OptimizerUnit<TBin>.WithHeuristic(
                possibleStockPieces,
                new ArraySegment<CutPieceWithId>(cutPiecesArr),
                bladeWidth,
                heuristic,
                rng));
        }
        if (cutPieces.Count > 2)
        {
            foreach (var heuristic in possibleHeuristics)
            {
                cutPiecesArr = cutPiecesArr.OrderBy(_ => rng.RandomInt()).ToArray();
                units.Add(OptimizerUnit<TBin>.WithHeuristic(
                    possibleStockPieces,
                    new ArraySegment<CutPieceWithId>(cutPiecesArr),
                    bladeWidth,
                    heuristic,
                    rng));
            }
            var curCount = units.Count;
            for (int i = 0; i < numUnits - curCount; i++)
            {
                cutPiecesArr = cutPiecesArr.OrderBy(_ => rng.RandomInt()).ToArray();
                units.Add(OptimizerUnit<TBin>.WithRandomHeuristics(
                    possibleStockPieces,
                    new ArraySegment<CutPieceWithId>(cutPiecesArr),
                    bladeWidth,
                    rng
                    ));
            }
        }

        return units;
    }
    private bool FirstFitWithHeuristics(CutPieceWithId cutPiece, IHeuristic heuristic, IRandomProvider rng)
    {
        foreach(var bin in Bins)
        {
            if(bin.InsertCutPieceWithHeuristic(cutPiece, heuristic))
            {
                return true;
            }
        }
        return AddToNewBin(cutPiece, rng);
    }

    private bool FirstFitRandomHeuristics(CutPieceWithId cutPiece, IRandomProvider rng)
    {
        foreach(var bin in Bins)
        {
            if(bin.InsertCutPieceRandomHeuristic(cutPiece, rng))
            {
                return true;
            }
        }

        return AddToNewBin(cutPiece, rng);
    }

    private bool AddToNewBin(CutPieceWithId cutPiece, IRandomProvider rng)
    {
        var stockPieces = AvailableStockPieces.Where(p => p.Quantity != 0 && p.FitsCutPiece(cutPiece)).ToList();
        if (stockPieces.Count == 0)
            return false;
        var stockPiece = rng.Choose(stockPieces);
        var bin = TBin.Create(stockPiece.Width,
            stockPiece.Length,
            BladeWidth,
            stockPiece.PatternDirection,
            stockPiece.Price);
        if (!bin.InsertCutPieceRandomHeuristic(cutPiece, rng))
            return false;
        Bins.Add((TBin)bin);
        return true;
    }
    OptimizerUnit<TBin> Clone()
    {
        return new OptimizerUnit<TBin>()
        {
            Bins = Bins.ToList(),
            PossibleStockPieces = PossibleStockPieces,
            AvailableStockPieces = AvailableStockPieces.ToList(),
            UnusedCutPieces = UnusedCutPieces.ToHashSet(),
        };
    }
    private OptimizerUnit<TBin> Crossover(OptimizerUnit<TBin> other, IRandomProvider rng)
    {
        if (Bins.Count < 2 && other.Bins.Count < 2)
        {
            return Clone();
        }

        var crossDest = rng.GenRange(0, Bins.Count+1);
        var crossSrcStart = rng.GenRange(0, other.Bins.Count);
        var crossSrcEnd = rng.GenRange(crossSrcStart + 1, other.Bins.Count + 1);

        var newUnit = new OptimizerUnit<TBin>()
        {
            Bins = (Bins.Count != 0 ? Bins[..crossDest] : Bins.ToList())
            .Concat(other.Bins.Count != 0 ? other.Bins[crossSrcStart..crossSrcEnd] : other.Bins.ToList())
            .Concat(Bins.Count != 0 ? Bins[crossDest..] : Bins.ToList())
            .ToList(),
            PossibleStockPieces = PossibleStockPieces,
            AvailableStockPieces = PossibleStockPieces.ToList()
        };

        var unused = UnusedCutPieces;

        foreach(var bin in other.Bins[crossSrcStart..crossSrcEnd])
        {
            // TODO: Is this correct?
            var applied = newUnit.AvailableStockPieces
                                    .ApplyToFirstStruct(bin.MatchesStockPiece, p => p.DecQuantity());
            if (!applied)
                throw new InvalidOperationException("Attempt to inject invalid bin in crossover operation");
        }

        var start = crossDest + crossSrcEnd - crossSrcStart;
        var end = newUnit.Bins.Count - (crossDest + crossSrcEnd - crossSrcStart);
        if(end >= start)
        {
            foreach(var i in Enumerable.Range(0, crossDest)
                .Concat(Enumerable.Range(start, end-start))
                .Reverse())
            {
                var bin = newUnit.Bins[i];
                var stockPiece = newUnit.AvailableStockPieces
                    .FirstOrNull(sp => sp.Quantity != 0 && bin.MatchesStockPiece(sp));
                var injectedCutPieces = other.Bins[crossSrcStart..crossSrcEnd]
                    .SelectMany(b => b.CutPieces()).ToList();
                var numRemovedCutPieces = bin.RemoveCutPieces(injectedCutPieces);
                if(numRemovedCutPieces == 0 && stockPiece != null)
                {
                    newUnit.AvailableStockPieces
                        .ApplyToFirstStruct(sp => sp.Quantity != 0 && bin.MatchesStockPiece(sp),sp => sp.DecQuantity());

                }
                else
                {
                    foreach(var cutPiece in bin.CutPieces())
                    {
                        unused.Add(cutPiece);
                    }
                    newUnit.Bins.RemoveAt(i);
                }
            }
        }
        unused = unused.Where(p => !newUnit.FirstFitRandomHeuristics(p, rng)).ToHashSet();
        newUnit.UnusedCutPieces = unused;

        foreach(var i in Enumerable.Range(0, newUnit.Bins.Count).Reverse())
        {
            if (!newUnit.Bins[i].CutPieces().Any())
            {
                var bin = newUnit.Bins[i];
                newUnit.AvailableStockPieces
                    .ApplyToFirstStruct(sp => sp.Quantity != 0 && bin.MatchesStockPiece(sp), sp => sp.IncQuantity());
                newUnit.Bins.RemoveAt(i);

            }
        }

        return newUnit;
    }

    public void Mutate(IRandomProvider rng)
    {
        if(Bins.Count > 0 && rng.GenRange(0, 20) == 1)
        {
            Inversion(rng);
        }
    }
    public void Inversion(IRandomProvider rng)
    {
        var start = rng.GenRange(0, Bins.Count);
        var end = rng.GenRange(start, Bins.Count);
        Bins.Reverse(start, end - start);
    }

    public OptimizerUnit<TBin> BreedWith(IUnit<OptimizerUnit<TBin>> other, IRandomProvider rng)
    {
        var newUnit = Crossover((OptimizerUnit<TBin>)other, rng);
        newUnit.Mutate(rng);
        return newUnit;
    }
}


public class Optimizer
{
    public List<StockPiece> StockPieces { get; set; } = new List<StockPiece>();
    public List<CutPieceWithId> CutPieces { get; set; } = new List<CutPieceWithId>();
    public int CutWidth { get; set; } = 0;
    public ulong RandomSeed { get; set; } = 0;
    public bool AllowMixedStockSizes { get; set; } = true;

    public Optimizer AddStockPiece(StockPiece stockPiece)
    {
        StockPiece _stockPiece = default;
        // TODO: Is this correct?
        if(StockPieces.TryFirstRef(sp =>
        {
            return sp.Width == stockPiece.Width &&
            sp.Length == stockPiece.Length &&
            sp.PatternDirection == stockPiece.PatternDirection &&
            sp.Price == stockPiece.Price;
        }, ref _stockPiece))
        {
            if(stockPiece.Quantity.HasValue && _stockPiece.Quantity.HasValue)
                _stockPiece.Quantity += stockPiece.Quantity;
            else
                _stockPiece.Quantity = null;
        }
        else
        {
            StockPieces.Add(stockPiece);
        }
        return this;
    }

    public Optimizer AddStockPieces(IEnumerable<StockPiece> stockPieces)
    {
        foreach(var stockPiece in stockPieces)
        {
            AddStockPiece(stockPiece);
        }
        return this;
    }

    public Optimizer AddCutPiece(CutPiece cutPiece)
    {
        for(int i = 0; i < cutPiece.Quantity; i++)
        {
            var newCutPiece = new CutPieceWithId()
            {
                Id = CutPieces.Count,
                ExternalId = cutPiece.ExternalId,
                Width = cutPiece.Width,
                Length = cutPiece.Length,
                PatternDirection = cutPiece.PatternDirection,
                CanRotate = cutPiece.CanRotate
            };
            CutPieces.Add(newCutPiece);
        }

        return this;
    }
    public Optimizer AddCutPieces(IEnumerable<CutPiece> cutPieces)
    {
        foreach(var cutPiece in cutPieces)
        {
            AddCutPiece(cutPiece);
        }
        return this;
    }


    public Solution? OptimizeGuillotine(Action<double> progressCallback)
    {
        return Optimize<GuillotineBin>(progressCallback);
    }

    public Solution? OptimizeNested(Action<double> progressCallback)
    {
        // TODO: Implement?
        return null;
    }

    internal Solution? Optimize<T>(Action<double> progressCallback) where T : IBin
    {
        if(CutPieces.Count == 0)
        {
            return new Solution()
            {
                Fitness = 1.0,
                Price = 0
            };
        }

        var sizeSet = StockPieces.Select(sp => (sp.Width, sp.Length)).ToHashSet();

        var numRuns = sizeSet.Count + (AllowMixedStockSizes ? 1 : 0);
        var callback = (double progress) =>
        {
            progressCallback((double)progress / (numRuns));
        };

        Solution? bestResult = null;
        if(AllowMixedStockSizes)
        {
            bestResult = OptimizeWithStockPieces<T>(StockPieces.ToArray(), callback);
        }

        int i = 0;
        foreach (var (width, length) in sizeSet)
        {
            var stockPieces = StockPieces.Where(sp => sp.Width == width && sp.Length == length).ToList();
            var completedRuns = i + 1;
            var solution = OptimizeWithStockPieces<T>(stockPieces.ToArray(), (progress) =>
            {
                progressCallback((completedRuns + progress) / numRuns);
            });

            if(solution is not null)
            {
                if(bestResult is not null)
                {
                    if(solution.Fitness < 0.0 || bestResult.Fitness < 0.0)
                    {
                        if(solution.Fitness > bestResult.Fitness)
                        {
                            bestResult = solution;
                        }
                    }
                    else if(solution.Price < bestResult.Price ||
                        (solution.Price == bestResult.Price && solution.Fitness > bestResult.Fitness))
                    {
                        bestResult = solution;
                    }
                }
                else
                {
                    bestResult = solution;
                }
            }
            i++;
        }

        if(bestResult is not null)
        {
            bestResult.StockPieces = bestResult.StockPieces.OrderByDescending(p => (p.Width, p.Length)).ToList();
        }
        return bestResult;
    }

    internal Solution? OptimizeWithStockPieces<T>(ArraySegment<StockPiece> stockPieces, Action<double> progressCallback) where T : IBin
    {
        var cutPieces = CutPieces;
        var units = OptimizerUnit<T>.GenerateInitialUnits(
            stockPieces,
            cutPieces,
            CutWidth,
            RandomSeed);
        var populationSize = units.Count;
        var resultUnits = new Population<OptimizerUnit<T>>(units.ToList())
        {
            MaxSize = populationSize,
            Seed = RandomSeed,
            BreedFactor = 0.5,
            SurvivalFactor = 0.6
        }.Epochs(100, progressCallback).Finish();

        var bestUnit = resultUnits[0];
        if (bestUnit.UnusedCutPieces.Count > 0)
        {
            // TODO: Throw or?
            throw new NoFitForCutPieceException(default);
        }
        var fitness = bestUnit.Fitness;
        var price = bestUnit.Bins.Select(bin => bin.Price()).Sum();

        var usedStockPieces = bestUnit.Bins.Select(b => b.IntoResultStockPiece()).ToList();
        return new Solution()
        {
            Fitness = fitness,
            StockPieces = usedStockPieces,
            Price = price
        };
    }
}

public class Solution
{
    public double Fitness { get; set; }
    public List<ResultStockPiece> StockPieces { get; set; } = new List<ResultStockPiece>();
    public int Price { get; set; }

}


class NoFitForCutPieceException : Exception
{ 
    public CutPiece Piece { get; set; }
    public NoFitForCutPieceException(CutPiece piece) : base("NoFItForCutPiece")
    {
        Piece = piece;
    }
}

