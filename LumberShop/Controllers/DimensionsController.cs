using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.BussinessLogic.Services;
using LumberStoreSystem.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LumberStoreSystem.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DimensionsController : ControllerBase
    {
        private readonly IDimensionsService _dimensionsService;
        public DimensionsController(IDimensionsService dimensionsService)
        {
            _dimensionsService = dimensionsService;
        }

        // GET: api/<ClientController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var dimensions = await _dimensionsService.GetAll();
                return Ok(dimensions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving dimensions");
            }
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var dimensions = await _dimensionsService.GetById(id);

                if (dimensions == null)
                {
                    return NotFound();
                }
                return Ok(dimensions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving client");
            }
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewDimensionsDTO dimensions)
        {
            try
            {
                await _dimensionsService.Add(dimensions);
                return Ok("Successfully added new dimensions");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        /// GET api/<AddressController>/FindByAll
        [HttpGet("FindByAll")]
        public async Task<IActionResult> FindByAll(int length, int width, int? height)
        {
            try
            {
                var dimensionsDTO = new DimensionsDTO
                {
                    Height = height,
                    Width = width,
                    Length = length
                };

                var dimensionsId = await _dimensionsService.FindByAll(dimensionsDTO); 

                return Ok(dimensionsId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving address: {ex.Message}");
            }
        }

        //// DELETE api/<ClientController>/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        await _dimensionsService.Delete(id);
        //        return Ok("Dimensions deleted successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred: {ex.Message}");
        //    }
        //}
    }
}
