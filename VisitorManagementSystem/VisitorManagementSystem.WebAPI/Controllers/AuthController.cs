using Microsoft.AspNetCore.Mvc;
using VisitorManagementSystem.Application.DTOs.Auth;
using VisitorManagementSystem.Infrastructure.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        var result = await _authService.RegisterAsync(model);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Ok(new { success = true });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var result = await _authService.LoginAsync(model);
        if (!(bool)result.GetType().GetProperty("success")!.GetValue(result))
            return Unauthorized(result);

        return Ok(result);
    }
}
