using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.BussinessLogic.Services;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Model;
using Microsoft.AspNetCore.Mvc;

namespace LumberStoreSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;
        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var orderItem = await _orderItemService.GetById(id);

                if (orderItem == null)
                {
                    return NotFound();
                }
                return Ok(orderItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving order item");
            }
        }

        // GET api/<EmployeeController>/5
        [HttpGet("byOrderId/{id}")]
        public async Task<IActionResult> GetByOrderId(int id)
        {
            try
            {
                var orderItem = await _orderItemService.GetByOrderId(id);

                if (orderItem == null)
                {
                    return NotFound();
                }
                return Ok(orderItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving order item");
            }
        }

        // POST api/<OrderItemController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewOrderItemDTO orderItem)
        {
            try
            {
                await _orderItemService.Add(orderItem);
                return Ok("Successfully added new order item");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // PUT api/<OrderItemController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] OrderItemDTO orderItem)
        {
            try
            {
                await _orderItemService.Update(orderItem);
                return Ok("Order item updated successfully.");
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
                await _orderItemService.Delete(id);
                return Ok("Order Item deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
