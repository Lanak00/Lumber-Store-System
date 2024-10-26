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

        [HttpPost("calculate-boards")]
        public IActionResult CalculateBoards([FromBody] CuttingRequest request)
        {
            try
            {
                int numberOfBoards = _cuttingService.CalculateNumberOfBoards(
                    request.BoardWidth,
                    request.BoardHeight,
                    request.CuttingList);

                var response = new CuttingResponse
                {
                    NumberOfBoards = numberOfBoards,
                    Message = "Calculation successful."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., no feasible solution, invalid input)
                var response = new CuttingResponse
                {
                    NumberOfBoards = -1,
                    Message = $"Error: {ex.Message}"
                };
                return BadRequest(response);
            }
        }
    }
    public class CuttingRequest
    {
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public List<CuttingListItemModel> CuttingList { get; set; }
    }

    public class CuttingResponse
    {
        public int NumberOfBoards { get; set; }
        public string Message { get; set; }
    }
}
