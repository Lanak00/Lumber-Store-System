using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.BussinessLogic.Services;
using LumberStoreSystem.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LumberStoreSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuttingListController : ControllerBase
    {
        private readonly ICuttingListService _cuttingListService;
        public CuttingListController(ICuttingListService cuttingListService)
        {
            _cuttingListService = cuttingListService;
        }

        // GET api/<EmployeeController>/5
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var cuttingLists = await _cuttingListService.GetAll();
                return Ok(cuttingLists);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving cutting lists");
            }
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var cuttingList = await _cuttingListService.GetById(id);

                if (cuttingList == null)
                {
                    return NotFound();
                }
                return Ok(cuttingList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving cutting list");
            }
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewCuttingListDTO cuttingList)
        {
            try
            {
                await _cuttingListService.Add(cuttingList);
                return Ok("Successfully added new cutting list");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // DELETE api/<ClientController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _cuttingListService.Delete(id);
                return Ok("Client deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
