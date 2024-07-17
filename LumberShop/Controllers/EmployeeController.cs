using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.BussinessLogic.Services;
using LumberStoreSystem.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LumberStoreSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
             _employeeService = employeeService;
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var employees = await _employeeService.GetAll();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving employees");
            }
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var employee = await _employeeService.GetById(id);

                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving employee");
            }
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewEmployeeDTO employee)
        {
            try
            {
                await _employeeService.Add(employee);
                return Ok("Successfully added new user");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // PUT api/<EmployeeController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] EmployeeDTO employee)
        {
            try
            {
                await _employeeService.Update(employee);
                return Ok("User updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _employeeService.Delete(id);
                return Ok("Employee deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
