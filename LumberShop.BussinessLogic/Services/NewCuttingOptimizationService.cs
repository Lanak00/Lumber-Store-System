using CutOptimizer;
using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.Contracts;
using Python.Runtime;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text;

public class NewCuttingOptimizationService : INewCuttingOptimizationService
{
    private readonly HttpClient _httpClient = new HttpClient();

    public string Optimize(int boardWidth, int boardHeight, List<CuttingListItemModel> cuttingList, string clientName, DateTime orderDate, int orderId, string productName, string productId)
    {
        var requestBody = new
        {
            boardWidth = boardWidth,
            boardHeight = boardHeight,
            cuttingList = cuttingList.Select(item => new { width = item.Width, length = item.Length, amount = item.Amount }).ToList()
        };
        var jsonContent = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = _httpClient.PostAsync("http://localhost:5000/api/optimize", content).Result;
        var responseContent = response.Content.ReadAsStringAsync().Result;

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Error in Python optimization: {response.Content.ReadAsStringAsync().Result}");
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var boardLayouts = JsonSerializer.Deserialize<List<BoardLayout>>(responseContent, options);


        if (boardLayouts == null)
        {
            throw new InvalidOperationException("Deserialization failed. Please check the response structure.");
        }

        return ExportToPdf(boardLayouts, boardWidth, boardHeight, productName, productId, clientName, orderId, orderDate);
    }

    public int GroupItemsIntoBoards(List<CuttingListItemModel> cuttingList, int boardWidth, int boardHeight)
    {
        var requestBody = new
        {
            boardWidth = boardWidth,
            boardHeight = boardHeight,
            cuttingList = cuttingList.Select(item => new { width = item.Width, length = item.Length, amount = item.Amount }).ToList()
        };
        var jsonContent = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = _httpClient.PostAsync("http://localhost:5000/api/board_count", content).Result;

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Error in Python grouping: {response.Content.ReadAsStringAsync().Result}");
        }

        var responseContent = response.Content.ReadAsStringAsync().Result;
        var result = JsonSerializer.Deserialize<Dictionary<string, int>>(responseContent);

        return result["numberOfBoards"];
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
