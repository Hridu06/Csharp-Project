using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VisitorManagementSystem.Application.DTOs.Auth;
using VisitorManagementSystem.Infrastructure.Identity;

namespace VisitorManagementSystem.Infrastructure.Services
{
    public class AuthService
    {
        private readonly IdentityService _identityService;

        public AuthService(IdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            return await _identityService.RegisterAsync(registerDto);
        }

        public async Task<object> LoginAsync(LoginDto loginDto)
        {
            var token = await _identityService.LoginAsync(loginDto);
            if (token == null) return new { success = false, message = "Invalid credentials" };
            return new { success = true, token };
        }
    }
}
