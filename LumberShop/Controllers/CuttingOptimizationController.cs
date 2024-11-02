using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Model;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LumberStoreSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuttingController : ControllerBase
    {
        private readonly ICuttingOptimizationService _cuttingService;

        public CuttingController(ICuttingOptimizationService cuttingService)
        {
            _cuttingService = cuttingService;
        }

        [HttpPost("CalculateBoards")]
        public IActionResult CalculateBoards([FromBody] CuttingRequest request)
        {
            try
            {
                var numberOfBoards = _cuttingService.GroupItemsIntoBoards( request.CuttingList, request.BoardWidth, request.BoardHeight);

                var response = new CuttingResponse
                {
                    NumberOfBoards = numberOfBoards.Count,
                    Message = "Calculation successful."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new CuttingResponse
                {
                    NumberOfBoards = -1,
                    Message = $"Error: {ex.Message}"
                };
                return BadRequest(response);
            }
        }

        [HttpPost("OptimizeAndGeneratePDF")]
        public IActionResult CalculateBoardsAndGeneratePdf([FromBody] CuttingRequest request)
        {
            try
            {
                var pdfPath = _cuttingService.Optimize(request.BoardWidth, request.BoardHeight, request.CuttingList, request.ClientFirstName + " " + request.ClientLastName, request.OrderDate, request.OrderId, request.ProductName, request.ProductId);

                if (!System.IO.File.Exists(pdfPath))
                {
                    return NotFound("PDF generation failed.");
                }

                var pdfBytes = System.IO.File.ReadAllBytes(pdfPath);

                System.IO.File.Delete(pdfPath);

                // Return the file as a downloadable response
                var result = new FileContentResult(pdfBytes, "application/pdf")
                {
                    FileDownloadName = "CuttingLayout.pdf"
                };

                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error: {ex.Message}" });
            }
        }

    }



    public class CuttingRequest
    {
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public List<CuttingListItemModel> CuttingList { get; set; }

        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public class CuttingResponse
    {
        public int NumberOfBoards { get; set; }
        public string Message { get; set; }
    }
};
