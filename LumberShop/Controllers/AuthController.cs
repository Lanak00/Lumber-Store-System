using LumberStoreSystem.BussinessLogic.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.BussinessLogic.Interfaces;

namespace LumberStoreSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IUserService _userService;
        public AuthController(TokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Contracts.LoginRequest request)
        {
            var user = await _userService.Authenticate(request.Username, request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            var token = _tokenService.GenerateToken(user.Email, user.Id);
            return Ok(new { token });
        }
    }
}
