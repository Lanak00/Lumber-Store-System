using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.BussinessLogic.Services;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LumberStoreSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;
        public OrderController(IOrderService orderService, IEmailService emailService)
        {
            _orderService = orderService;
            _emailService = emailService;
        }

        [HttpGet]
        [Authorize(Roles = "Employee, Administrator")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var orders = await _orderService.GetAll();
                return Ok(orders);
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
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving order");
            }
        }

        [HttpGet("byClientId/{id}")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetByClientId(int id)
        {
            try
            {
                var orders = await _orderService.GetByClientId(id);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving orders");
            }
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Post([FromBody] NewOrderDTO order)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value
                    ?? User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized("Client email not found.");
                }

                await _orderService.Add(order);

                string subject = "Potvrda Porudzbine";
                string body = "Hvala na poduzdbini! Pripremicemo je sto pre i javiti kada bude dostupna za preuzimanje";
                await _emailService.SendEmailAsync(email, subject, body);
                var response = new { message = "Uspesno kreirana nova porudzbina. Email sa potvrrdom je poslat na vasu adresu." };

                return Ok(response);
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
