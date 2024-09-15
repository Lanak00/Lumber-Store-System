using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.BussinessLogic.Services;
using LumberStoreSystem.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace LumberStoreSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        // GET api/<AddressController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var address = await _addressService.GetById(id);

                if (address == null)
                {
                    return NotFound();
                }
                return Ok(address);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving address");
            }
        }

        /// GET api/<AddressController>/FindByAll
        [HttpGet("FindByAll")]
        public async Task<IActionResult> FindByAll(string city, string country, string street, string number)
        {
            try
            {
                var addressDTO = new AddressDTO
                {
                    Street = street,
                    Number = number,
                    City = city,
                    Country = country
                };

                var addressId = await _addressService.FindByAll(addressDTO); // Await the asynchronous call

                return Ok(addressId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving address: {ex.Message}");
            }
        }

        // POST api/<AddressController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewAddressDTO address)
        {
            try
            {
                var newAddressId = await _addressService.Add(address);
                return Ok(new { Message = "Successfully added new address", Id = newAddressId });
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
                await _addressService.Delete(id);
                return Ok("Address deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
