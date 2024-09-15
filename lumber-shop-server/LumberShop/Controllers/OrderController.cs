using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.BussinessLogic.Services;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Model;
using Microsoft.AspNetCore.Mvc;

namespace LumberStoreSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var order = await _orderService.GetAll();
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving order");
            }
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var order = await _orderService.GetById(id);

                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving order");
            }
        }

        // GET api/<EmployeeController>/5
        [HttpGet("byClientId/{id}")]
        public async Task<IActionResult> GetByClientId(int id)
        {
            try
            {
                var order = await _orderService.GetByClientId(id);

                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving order");
            }
        }

        // POST api/<OrderItemController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewOrderDTO order)
        {
            try
            {
                await _orderService.Add(order);
                return Ok("Successfully added new order");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // PUT api/<OrderItemController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] OrderDTO order)
        {
            try
            {
                await _orderService.Update(order);
                return Ok("Order updated successfully.");
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
                await _orderService.Delete(id);
                return Ok("Order deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
