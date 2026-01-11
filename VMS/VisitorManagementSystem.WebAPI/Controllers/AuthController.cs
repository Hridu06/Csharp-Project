using Microsoft.AspNetCore.Mvc;
using VisitorManagementSystem.Application.DTOs;
using VisitorManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace VisitorManagementSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _authService;

        public AuthController(IUserService authService)
        {
            _authService = authService;
        }

        // ✅ Anyone can register (Admin, User, Employee, etc.)
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            // Includes UserId and FullName now
            return Ok(result);
        }

        // ✅ Anyone can log in
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (!result.Success)
                return Unauthorized(result);

            // Includes UserId and FullName now
            return Ok(result);
        }

        // 🔒 Admin-only endpoint
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("✅ This endpoint is accessible only to Admin users.");
        }

        // 👥 User-only endpoint
        [Authorize(Roles = "User")]
        [HttpGet("user-only")]
        public IActionResult UserOnlyEndpoint()
        {
            return Ok("✅ This endpoint is accessible only to regular User role.");
        }

        // 🧑‍💼 Employee-only endpoint
        [Authorize(Roles = "Employee")]
        [HttpGet("employee-only")]
        public IActionResult EmployeeOnlyEndpoint()
        {
            return Ok("✅ This endpoint is accessible only to Employee role.");
        }

        // 👥 Shared between Admin + User + Employee
        [Authorize(Roles = "Admin,User,Employee")]
        [HttpGet("shared-endpoint")]
        public IActionResult SharedEndpoint()
        {
            return Ok("✅ This endpoint is accessible to Admin, User, and Employee roles.");
        }
    }
}
