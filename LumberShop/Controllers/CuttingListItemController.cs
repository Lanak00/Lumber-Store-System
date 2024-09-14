using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.BussinessLogic.Services;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Model;
using Microsoft.AspNetCore.Mvc;

namespace LumberStoreSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuttingListItemController : ControllerBase
    {
        private readonly ICuttingListItemService _cuttingListItemService;
        public CuttingListItemController(ICuttingListItemService cuttingListItemService)
        {
            _cuttingListItemService = cuttingListItemService;
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var cuttingListItem = await _cuttingListItemService.GetById(id);

                if (cuttingListItem == null)
                {
                    return NotFound();
                }
                return Ok(cuttingListItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving cutting list item");
            }
        }

        // GET api/<EmployeeController>/5
        /*[HttpGet("byOrderId/{id}")]
        public async Task<IActionResult> GetByCuttingListId(int id)
        {
            try
            {
                var cuttingListItem = await _cuttingListItemService.GetByOrderId(id);

                if (cuttingListItem == null)
                {
                    return NotFound();
                }
                return Ok(cuttingListItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving cutting list item");
            }
        }*/

        // POST api/<OrderItemController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewCuttingListItemDTO cuttingListItem)
        {
            try
            {
                await _cuttingListItemService.Add(cuttingListItem);
                return Ok("Successfully added new cutting list item");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // PUT api/<OrderItemController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CuttingListItemDTO cuttingListItem)
        {
            try
            {
                await _cuttingListItemService.Update(cuttingListItem);
                return Ok("Cutting list item updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // DELETE api/<OrderItemController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _cuttingListItemService.Delete(id);
                return Ok("Cutting list Item deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
