using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CutOptimizer;

public enum FreeRectChoiceHeuristic
{
    BestAreaFit,
    BestShortSideFit,
    BestLongSideFit,
    WorstAreaFit,
    WorstShortSideFit,
    WorstLongSideFit,
    SmallestY
}

public enum SplitHeuristic
{
    ShorterLeftoverAxis,
    LongerLeftoverAxis,
    MinimizeArea,
    MaximizeArea,
    ShorterAxis,
    LongerAxis,
}

enum SplitAxis
{
    Horizontal,
    Vertical,
}

public enum RotateCutPieceHeuristic
{
    PreferUpright,
    PreferRotated,
}

internal static class GuillotineUtils
{
    public static FreeRectChoiceHeuristic SampleRectChoice(this IRandomProvider rng)
    {
        return rng.GenRange(0, 3) switch
        {
            0 => FreeRectChoiceHeuristic.BestAreaFit,
            1 => FreeRectChoiceHeuristic.BestShortSideFit,
            _ => FreeRectChoiceHeuristic.BestLongSideFit
        };
    }
    public static SplitHeuristic SampleSplit(this IRandomProvider rng)
    {
        return rng.GenRange(0, 6) switch
        {
            0 => SplitHeuristic.ShorterLeftoverAxis,
            1 => SplitHeuristic.LongerLeftoverAxis,
            2 => SplitHeuristic.MinimizeArea,
            3 => SplitHeuristic.MaximizeArea,
            4 => SplitHeuristic.ShorterAxis,
            _ => SplitHeuristic.LongerAxis,
        };
    }

    public static RotateCutPieceHeuristic SampleRotateCut(this IRandomProvider rng)
    {
        return rng.GenRange(0, 2) switch
        {
            0 => RotateCutPieceHeuristic.PreferUpright,
            _ => RotateCutPieceHeuristic.PreferRotated,
        };
    }
}


public class GuillotineHeuristic : IHeuristic
{
    public FreeRectChoiceHeuristic FreeRectChoiceHeuristic { get; set; }
    public SplitHeuristic SplitHeuristic { get; set; }
    public RotateCutPieceHeuristic RotateCutPieceHeuristic { get; set; }

    public GuillotineHeuristic(
        FreeRectChoiceHeuristic freeRectChoiceHeuristic,
        SplitHeuristic splitHeuristic,
        RotateCutPieceHeuristic rotateCutPieceHeuristic)
    {
        FreeRectChoiceHeuristic = freeRectChoiceHeuristic;
        SplitHeuristic = splitHeuristic;
        RotateCutPieceHeuristic = rotateCutPieceHeuristic;
    }
}
public class GuillotineBin : IBin
{
    public int Width { get; set; }
    public int Length { get; set; }
    public int BladeWidth => 0;
    public PatternDirection PatternDirection { get; set; }
    public List<UsedCutPiece> CutPieces { get; } = new List<UsedCutPiece>();
    public List<Rect> FreeRects { get; } = new List<Rect>();
    public int Price { get; set; }

    public GuillotineBin(int width, int length, PatternDirection patternDirection, int price)
    {
        Width = width;
        Length = length;
        PatternDirection = patternDirection;
        Price = price;
        var freeRect = new Rect(0, 0, width, length);
        FreeRects.Add(freeRect);
    }

    public static IBin Create(int width, int length, int bladeWidth, PatternDirection patternDirection, int price)
    {
        return new GuillotineBin(width, length, patternDirection, price);
    }
    public ResultStockPiece IntoResultStockPiece()
    {
        return new ResultStockPiece()
        {
            Width = Width,
            Length = Length,
            PatternDirection = PatternDirection,
            CutPieces = CutPieces.Select(c => (ResultCutPiece)c).ToList(),
            WastePieces = FreeRects.ToList(),
            Price = Price
        };
    }
    public double Fitness()
    {
        var usedArea = (double)CutPieces.Aggregate(0.0, (acc, p) => acc + p.Rect.Width * p.Rect.Length);

        var freeArea = (double)FreeRects.Aggregate(0.0, (acc, fr) => acc + fr.Width * fr.Length);
        return Math.Pow((usedArea / (usedArea + freeArea)), 2.0 + FreeRects.Count() * 0.01);
    }

    int IBin.Price()
    {
        return Price;
    }

    public int RemoveCutPieces<TCut>(IEnumerable<TCut> cutPieces) where TCut : UsedCutPiece
    {
        var oldLen = CutPieces.Count;
        foreach (var cutPieceToRemove in cutPieces)
        {
            foreach (var i in Enumerable.Range(0, CutPieces.Count).Reverse())
            {
                if (CutPieces[i] == cutPieceToRemove)
                {
                    var removedPiece = CutPieces[i];
                    CutPieces.RemoveAt(i);
                    FreeRects.Add(removedPiece.Rect);
                }
            }
        }
        MergeFreeRects();
        return oldLen - CutPieces.Count;
    }



    IEnumerable<UsedCutPiece> IBin.CutPieces()
    {
        return CutPieces;
    }

    public static List<IHeuristic> PossibleHeuristics()
    {
        return new List<IHeuristic>()
        {
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestAreaFit,
                SplitHeuristic.ShorterLeftoverAxis,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestAreaFit,
                SplitHeuristic.LongerLeftoverAxis,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestAreaFit,
                SplitHeuristic.MinimizeArea,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestAreaFit,
                SplitHeuristic.MaximizeArea,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestAreaFit,
                SplitHeuristic.ShorterAxis,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestAreaFit,
                SplitHeuristic.LongerAxis,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestShortSideFit,
                SplitHeuristic.ShorterLeftoverAxis,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestShortSideFit,
                SplitHeuristic.LongerLeftoverAxis,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestShortSideFit,
                SplitHeuristic.MinimizeArea,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestShortSideFit,
                SplitHeuristic.MaximizeArea,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestShortSideFit,
                SplitHeuristic.ShorterAxis,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestShortSideFit,
                SplitHeuristic.LongerAxis,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestLongSideFit,
                SplitHeuristic.ShorterLeftoverAxis,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestLongSideFit,
                SplitHeuristic.LongerLeftoverAxis,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestLongSideFit,
                SplitHeuristic.MinimizeArea,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestLongSideFit,
                SplitHeuristic.MaximizeArea,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestLongSideFit,
                SplitHeuristic.ShorterAxis,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestLongSideFit,
                SplitHeuristic.LongerAxis,
                RotateCutPieceHeuristic.PreferUpright
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestAreaFit,
                SplitHeuristic.ShorterLeftoverAxis,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestAreaFit,
                SplitHeuristic.LongerLeftoverAxis,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestAreaFit,
                SplitHeuristic.MinimizeArea,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestAreaFit,
                SplitHeuristic.MaximizeArea,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestAreaFit,
                SplitHeuristic.ShorterAxis,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestAreaFit,
                SplitHeuristic.LongerAxis,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestShortSideFit,
                SplitHeuristic.ShorterLeftoverAxis,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestShortSideFit,
                SplitHeuristic.LongerLeftoverAxis,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestShortSideFit,
                SplitHeuristic.MinimizeArea,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestShortSideFit,
                SplitHeuristic.MaximizeArea,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestShortSideFit,
                SplitHeuristic.ShorterAxis,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestShortSideFit,
                SplitHeuristic.LongerAxis,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestLongSideFit,
                SplitHeuristic.ShorterLeftoverAxis,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestLongSideFit,
                SplitHeuristic.LongerLeftoverAxis,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestLongSideFit,
                SplitHeuristic.MinimizeArea,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestLongSideFit,
                SplitHeuristic.MaximizeArea,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestLongSideFit,
                SplitHeuristic.ShorterAxis,
                RotateCutPieceHeuristic.PreferRotated
            ),
            new GuillotineHeuristic(
                FreeRectChoiceHeuristic.BestLongSideFit,
                SplitHeuristic.LongerAxis,
                RotateCutPieceHeuristic.PreferRotated
            )
        };
    }

    public bool InsertCutPieceWithHeuristic(CutPieceWithId cutPiece, IHeuristic heuristic)
    {
        var h = (GuillotineHeuristic)heuristic;
        return InsertWithHeuristic(cutPiece, true, h.FreeRectChoiceHeuristic,
            h.SplitHeuristic, h.RotateCutPieceHeuristic);
    }

    private bool InsertWithHeuristic(
        CutPieceWithId cutPiece,
        bool merge,
        FreeRectChoiceHeuristic rectChoice,
        SplitHeuristic splitMethod,
        RotateCutPieceHeuristic rotatePreference)
    {
        var preferRotated = rotatePreference == RotateCutPieceHeuristic.PreferRotated;

        if (FindPlacementForCutPiece(cutPiece, rectChoice, preferRotated) is var (usedPiece, freeIndex))
        {

            var freeRect = FreeRects[freeIndex];
            // TODO: swap remove?
            FreeRects.RemoveAt(freeIndex);
            SplitFreeRectByHeuristic(freeRect, usedPiece.Rect, splitMethod);
            if (merge)
            {
                MergeFreeRects();
            }
            CutPieces.Add(usedPiece);
            return true;
        }
        return false;
    }



    public (UsedCutPiece usedPiece, int freeIndex)? FindPlacementForCutPiece(
        CutPieceWithId cutPiece,
        FreeRectChoiceHeuristic rectChoice,
        bool preferRotated)
    {
        var bestRect = default(Rect);
        var bestScore = int.MaxValue;
        var score = 0;
        var bestFit = Fit.None;
        int? freeIndex = null;
        bool done = false;

        for (int i = 0; i < FreeRects.Count; i++)
        {
            var freeRect = FreeRects[i];
            var fit = freeRect.FitCutPiece(PatternDirection, cutPiece, preferRotated);
            switch (fit)
            {
                case Fit.UprightExact:
                    bestRect.X = freeRect.X;
                    bestRect.Y = freeRect.Y;
                    bestRect.Width = cutPiece.Width;
                    bestRect.Length = cutPiece.Length;
                    bestFit = fit;
                    freeIndex = i;
                    done = true;
                    break;
                case Fit.RotatedExact:
                    bestRect.X = freeRect.X;
                    bestRect.Y = freeRect.Y;
                    bestRect.Width = cutPiece.Length;
                    bestRect.Length = cutPiece.Width;
                    bestFit = fit;
                    freeIndex = i;
                    done = true;
                    break;
                case Fit.Upright:
                    score = ScoreByHeuristic(cutPiece.Width,
                        cutPiece.Length,
                        freeRect,
                        rectChoice);
                    if (score < bestScore)
                    {
                        bestRect.X = freeRect.X;
                        bestRect.Y = freeRect.Y;
                        bestRect.Width = cutPiece.Width;
                        bestRect.Length = cutPiece.Length;
                        bestScore = score;
                        bestFit = fit;
                        freeIndex = i;
                    }
                    break;
                case Fit.Rotated:
                    score = ScoreByHeuristic(cutPiece.Length,
                                            cutPiece.Width,
                                            freeRect,
                                            rectChoice);
                    if (score < bestScore)
                    {
                        bestRect.X = freeRect.X;
                        bestRect.Y = freeRect.Y;
                        bestRect.Width = cutPiece.Length;
                        bestRect.Length = cutPiece.Width;
                        bestScore = score;
                        bestFit = fit;
                        freeIndex = i;
                    }
                    break;
                case Fit.None:
                    break;
            }

            if (done)
                break;
        }

        if (freeIndex is not null)
        {
            var isRotated = bestFit == Fit.Rotated || bestFit == Fit.RotatedExact;
            var patternDirection = cutPiece.PatternDirection;
            if (isRotated)
                patternDirection = patternDirection.Rotated();

            return (new UsedCutPiece()
            {
                Id = cutPiece.Id,
                ExternalId = cutPiece.ExternalId,
                Rect = bestRect,
                CanRotate = cutPiece.CanRotate,
                PatternDirection = patternDirection,
                IsRotated = isRotated
            }, freeIndex.Value);
        }
        return null;

    }
    private void SplitFreeRectByHeuristic(Rect freeRect, Rect rect, SplitHeuristic method)
    {
        var w = (ulong)(freeRect.Width - rect.Width);
        var h = (ulong)(freeRect.Length - rect.Length);
        var splitHorizontal = method switch
        {
            SplitHeuristic.ShorterLeftoverAxis => w <= h,
            SplitHeuristic.LongerLeftoverAxis => w > h,
            SplitHeuristic.MinimizeArea => (ulong)rect.Width * h > w * (ulong)rect.Length,
            SplitHeuristic.MaximizeArea => (ulong)rect.Width * h <= w * (ulong)rect.Length,
            SplitHeuristic.ShorterAxis => (ulong)freeRect.Width <= (ulong)freeRect.Length,
            SplitHeuristic.LongerAxis => (ulong)freeRect.Width > (ulong)freeRect.Length,
            _ => throw new InvalidOperationException()
        };

        var splitAxis = splitHorizontal ? SplitAxis.Horizontal : SplitAxis.Vertical;

        SplitFreeRectAlongAxis(freeRect, rect, splitAxis);
    }

    private void SplitFreeRectAlongAxis(Rect freeRect, Rect rect, SplitAxis splitAxis)
    {
        var (bottomWidth, rightLength) = splitAxis switch
        {
            SplitAxis.Horizontal => (freeRect.Width, rect.Length),
            SplitAxis.Vertical => (rect.Width, freeRect.Length),
            _ => throw new InvalidOperationException()
        };

        var bottomLength = freeRect.Length - rect.Length;
        var rightWidth = freeRect.Width - rect.Width;

        if (bottomWidth > 0 && bottomLength > 0)
        {
            var bottom = new Rect(freeRect.X, freeRect.Y + rect.Length, bottomWidth, bottomLength);
            FreeRects.Add(bottom);
        }

        if (rightWidth > 0 && rightLength > 0)
        {
            var right = new Rect(freeRect.X + rect.Width, freeRect.Y, rightWidth, rightLength);
            FreeRects.Add(right);
        }
    }

    private void MergeFreeRects()
    {
        foreach (var i in Enumerable.Range(0, FreeRects.Count).Reverse())
        {
            foreach (var j in Enumerable.Range(i + 1, FreeRects.Count - (i+1)).Reverse())
            {
                if (FreeRects[i].Width == FreeRects[j].Width &&
                    FreeRects[i].X == FreeRects[j].X)
                {
                    if (FreeRects[i].Y == FreeRects[j].Y + FreeRects[j].Length)
                    {
                        var rect = FreeRects[i];
                        rect.Y -= FreeRects[j].Length;
                        rect.Length += FreeRects[j].Length;
                        FreeRects[i] = rect;
                        FreeRects.SwapRemoveAt(j);
                    }
                    else if (FreeRects[i].Y + FreeRects[i].Length == FreeRects[j].Y)
                    {
                        var rect = FreeRects[i];
                        rect.Length += FreeRects[j].Length;
                        FreeRects[i] = rect;
                        FreeRects.SwapRemoveAt(j);
                    }
                }
                else if (FreeRects[i].Length == FreeRects[j].Length &&
                    FreeRects[i].Y == FreeRects[j].Y)
                {
                    if (FreeRects[i].X == FreeRects[j].X + FreeRects[j].X)
                    {
                        var rect = FreeRects[i];
                        rect.X -= FreeRects[j].Width;
                        rect.Width += FreeRects[j].Width;
                        FreeRects[i] = rect;
                        FreeRects.SwapRemoveAt(j);
                    }
                    else if (FreeRects[i].X + FreeRects[i].Width == FreeRects[j].X)
                    {
                        var rect = FreeRects[i];
                        rect.Width += FreeRects[j].Width;
                        FreeRects[i] = rect;
                        FreeRects.SwapRemoveAt(j);
                    }
                }
            }
        }
    }

    

    public bool InsertCutPieceRandomHeuristic(CutPieceWithId cutPiece, IRandomProvider rng)
    {
        return InsertCutPieceWithHeuristic(cutPiece, new GuillotineHeuristic(
            rng.SampleRectChoice(),
            rng.SampleSplit(),
            rng.SampleRotateCut()));
    }

    public bool MatchesStockPiece(StockPiece stockPiece)
    {
        return Width == stockPiece.Width &&
            Length == stockPiece.Length &&
            PatternDirection == stockPiece.PatternDirection &&
            Price == stockPiece.Price;
    }

    public static implicit operator ResultStockPiece(GuillotineBin bin)
    {
        return new ResultStockPiece()
        {
            Width = bin.Width,
            Length = bin.Length,
            PatternDirection = bin.PatternDirection,
            CutPieces = bin.CutPieces.Select(c => (ResultCutPiece)c).ToList(),
            WastePieces = bin.FreeRects.ToList(),
            Price = bin.Price,
        };
    }

    private int ScoreByHeuristic(
        int width,
        int length,
        Rect freeRect,
        FreeRectChoiceHeuristic rectChoice)
    {
        return rectChoice switch
        {
            FreeRectChoiceHeuristic.BestAreaFit => ScoreBestAreaFit(width, length, freeRect),
            FreeRectChoiceHeuristic.BestShortSideFit => ScoreBestShortSideFit(width, length, freeRect),
            FreeRectChoiceHeuristic.BestLongSideFit => ScoreBestLongSideFit(width, length, freeRect),
            FreeRectChoiceHeuristic.WorstAreaFit => ScoreWorstAreaFit(width, length, freeRect),
            FreeRectChoiceHeuristic.WorstShortSideFit => ScoreWorstShortestSideFit(width, length, freeRect),
            FreeRectChoiceHeuristic.WorstLongSideFit => ScoreWorstLongSideFit(width, length, freeRect),
            FreeRectChoiceHeuristic.SmallestY => freeRect.Y,
            _ => throw new InvalidOperationException()
        };
    }

    private int ScoreWorstLongSideFit(int width, int length, Rect freeRect)
    {
        return -ScoreBestLongSideFit(width, length, freeRect);
    }

    private int ScoreWorstShortestSideFit(int width, int length, Rect freeRect)
    {
        return -ScoreBestShortSideFit(width, length, freeRect);
    }

    private int ScoreWorstAreaFit(int width, int length, Rect freeRect)
    {
        return -ScoreBestAreaFit(width, length, freeRect);
    }

    private int ScoreBestLongSideFit(int width, int length, Rect freeRect)
    {
        var leftoverHoriz = Math.Abs(freeRect.Width - (long)width);
        var leftOverVert = Math.Abs(freeRect.Length - (long)length);
        return (int)Math.Max(leftOverVert, leftoverHoriz);
    }

    private int ScoreBestShortSideFit(int width, int length, Rect freeRect)
    {
        var leftoverHoriz = Math.Abs(freeRect.Width - (long)width);
        var leftOverVert = Math.Abs(freeRect.Length - (long)length);
        return (int)Math.Min(leftOverVert, leftoverHoriz);
    }

    private int ScoreBestAreaFit(int width, int length, Rect freeRect)
    {
        return (int)((freeRect.Width * (long)freeRect.Length) - (width * (long)length));
    }
}
