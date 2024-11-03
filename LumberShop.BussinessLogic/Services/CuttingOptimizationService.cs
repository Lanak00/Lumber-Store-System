using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.Contracts;
using SkiaSharp;
using CutOptimizer;

public class CuttingOptimizationService : ICuttingOptimizationService
    {
    public string Optimize(int boardWidth, int boardHeight, List<CuttingListItemModel> cuttingList, string clientName, DateTime orderDate, int orderId, string productName, string productId)
    {
        var sortedCuttingList = cuttingList.OrderByDescending(item => item.Width * item.Length).ToList();

        var groupedCuttingLists = GroupItemsIntoBoards(sortedCuttingList, boardWidth, boardHeight);
        var boardLayouts = new List<BoardLayout>();

        foreach (var group in groupedCuttingLists)
        {
            var optimizer = new Optimizer()
                .AddStockPieces(new List<StockPiece> {
                    new StockPiece
                    {
                        Width = boardWidth,
                        Length = boardHeight,
                        PatternDirection = PatternDirection.None,
                        Price = 0,
                        Quantity = null
                    }
                })
                .AddCutPieces(group);

            optimizer.CutWidth = 0;
            optimizer.RandomSeed = 1;

            var solution = optimizer.OptimizeGuillotine(_ => { });

            foreach (var stockPiece in solution.StockPieces)
            {
                if (stockPiece.CutPieces.Count > 0)
                {
                    var boardLayout = new BoardLayout();
                    foreach (var cutPiece in stockPiece.CutPieces)
                    {
                        boardLayout.Pieces.Add(new PiecePosition
                        {
                            X = cutPiece.X,
                            Y = cutPiece.Y,
                            Width = cutPiece.Width,
                            Length = cutPiece.Length
                        });
                    }
                    boardLayouts.Add(boardLayout);
                }
            }
        }

        return ExportToPdf(boardLayouts, boardWidth, boardHeight, productName, productId, clientName, orderId, orderDate);
    }

    public List<List<CutPiece>> GroupItemsIntoBoards(List<CuttingListItemModel> cuttingList, int boardWidth, int boardHeight)
    {
        var groupedCuttingLists = new List<List<CutPiece>>();
        var currentGroup = new List<CutPiece>();
        var remainingPieces = new List<CutPiece>();

        foreach (var item in cuttingList)
        {
            for (int i = 0; i < item.Amount; i++)
            {
                var cutPiece = new CutPiece
                {
                    Width = item.Width,
                    Length = item.Length,
                    Quantity = 1,
                    ExternalId = i,
                    PatternDirection = PatternDirection.None,
                    CanRotate = true
                };

                if (CanFitPieceInGroup(currentGroup, cutPiece, boardWidth, boardHeight))
                {
                    currentGroup.Add(cutPiece);
                }
                else
                {
                    remainingPieces.Add(cutPiece);
                }
            }
        }

        // Add the current group to the list if it has any pieces
        if (currentGroup.Count > 0)
        {
            groupedCuttingLists.Add(new List<CutPiece>(currentGroup));
            currentGroup.Clear();
        }

        // Second pass: Try to fit remaining pieces into existing groups before creating new ones
        foreach (var piece in remainingPieces)
        {
            bool pieceAdded = false;
            foreach (var group in groupedCuttingLists)
            {
                if (CanFitPieceInGroup(group, piece, boardWidth, boardHeight))
                {
                    group.Add(piece);
                    pieceAdded = true;
                    break;
                }
            }

            // If the piece couldn't be added to any existing group, start a new group
            if (!pieceAdded)
            {
                currentGroup.Add(piece);
                groupedCuttingLists.Add(new List<CutPiece>(currentGroup));
                currentGroup.Clear();
            }
        }

        return groupedCuttingLists;
    }

    private bool CanFitPieceInGroup(List<CutPiece> currentGroup, CutPiece newPiece, int boardWidth, int boardHeight)
    {
        var optimizer = new Optimizer()
            .AddStockPieces(new List<StockPiece> {
                new StockPiece
                {
                    Width = boardWidth,
                    Length = boardHeight,
                    PatternDirection = PatternDirection.None,
                    Price = 0,
                    Quantity = null
                }
            })
            .AddCutPieces(currentGroup);

        optimizer.AddCutPiece(newPiece);
        optimizer.CutWidth = 0;
        optimizer.RandomSeed = 1;

        var solution = optimizer.OptimizeGuillotine(_ => { });

        // If the solution fits in one board, return true
        return solution.StockPieces.Count > 0 && solution.StockPieces[0].CutPieces.Count == currentGroup.Count + 1;
    }

    private string ExportToPdf(List<BoardLayout> boardLayouts, int boardWidth, int boardHeight, string productName, string productId, string clientName, int orderId, DateTime orderDate)
    {
        var tempPdfPath = Path.Combine(Path.GetTempPath(), $"CuttingLayout_{Guid.NewGuid()}.pdf");

        // A4 page size in points (1 point = 1/72 inch)
        const float a4Width = 595f;
        const float a4Height = 842f;

        using var document = SKDocument.CreatePdf(tempPdfPath);
        foreach (var layout in boardLayouts)
        {

            var canvas = document.BeginPage(a4Width, a4Height);

            DrawMainHeading(canvas, orderId, a4Width);

            DrawHeader(canvas, productName, productId, clientName, orderId, orderDate, 80); 

            float scaleFactor = Math.Min((a4Width - 40) / boardWidth, (a4Height - 200) / boardHeight) * 0.8f;

            float boardX = (a4Width - (boardWidth * scaleFactor)) / 2;
            float boardY = 200; 

            DrawLayout(canvas, layout, boardX, boardY, boardWidth, boardHeight, scaleFactor);

            document.EndPage();
        }
        document.Close();

        return tempPdfPath;
    }



    private void DrawLayout(SKCanvas canvas, BoardLayout layout, float boardX, float boardY, int boardWidth, int boardHeight, float scaleFactor)
    {
        // Draw the outer rectangle for the board
        var boardRect = new SKRect(boardX, boardY, boardX + boardWidth * scaleFactor, boardY + boardHeight * scaleFactor);
        var paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 2,
            Color = SKColors.Black
        };
        canvas.DrawRect(boardRect, paint);

        // Draw labels for board dimensions on the outer edges
        var textPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 12,
            IsAntialias = true
        };

        canvas.DrawText($"{boardWidth} x {boardHeight}", boardX + (boardRect.Width / 2) - 20, boardY - 10, textPaint);

        // Draw each piece within the board
        foreach (var piece in layout.Pieces)
        {
            var pieceRect = new SKRect(
                boardX + piece.X * scaleFactor,
                boardY + piece.Y * scaleFactor,
                boardX + (piece.X + piece.Width) * scaleFactor,
                boardY + (piece.Y + piece.Length) * scaleFactor
            );

            canvas.DrawRect(pieceRect, paint);

            // Draw piece dimensions inside each piece
            canvas.DrawText($"{piece.Width} x {piece.Length}", pieceRect.Left + 5, pieceRect.Top + 15, textPaint);
        }
    }

    private void DrawMainHeading(SKCanvas canvas, int orderId, float pageWidth)
    {
        var headingText = $"Porudzbina br. {orderId}";
        var textPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 24, // Larger font size for main heading
            IsAntialias = true,
            TextAlign = SKTextAlign.Center
        };

        // Draw text centered at the top of the page
        canvas.DrawText(headingText, pageWidth / 2, 40, textPaint);
    }

    private void DrawHeader(SKCanvas canvas, string productName, string productId, string clientName, int orderId, DateTime orderDate, float yPosition = 80)
    {
        var headerText = $"Proizvod: {productName}\n" +
                         $"Šifra: {productId}\n" +
                         $"Klijent: {clientName}\n" +
                         $"Broj porudzbine: {orderId}\n" +
                         $"Datum porudzbine: {orderDate:yyyy-MM-dd}";

        var textPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 12, // Smaller font size for header details
            IsAntialias = true
        };

        float xPosition = 20;

        foreach (var line in headerText.Split('\n'))
        {
            canvas.DrawText(line, xPosition, yPosition, textPaint);
            yPosition += textPaint.TextSize + 5; // Smaller spacing between lines
        }
    }
}

public class BoardLayout
{
    public List<PiecePosition> Pieces { get; set; } = new List<PiecePosition>();
}

public class PiecePosition
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Length { get; set; }
}

