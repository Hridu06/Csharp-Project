using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VisitorManagementSystem.Application.DTOs;
using VisitorManagementSystem.Application.Interfaces;
using VisitorManagementSystem.Domain.Entities;
using VisitorManagementSystem.Infrastructure.Configurations;
using VisitorManagementSystem.Infrastructure.Data;

namespace VisitorManagementSystem.Infrastructure.Services
{
    public class AuthService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthService(UserManager<User> userManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
        }

        // ✅ Register new user
        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            // 1️⃣ Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (existingUser != null)
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = "User already exists"
                };
            }

            // 2️⃣ Create new User
            var newUser = new User
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                FullName = registerDTO.FullName
            };

            var result = await _userManager.CreateAsync(newUser, registerDTO.Password);
            if (!result.Succeeded)
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            // 3️⃣ Determine role
            string roleToAssign;

            if (registerDTO.Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true)
            {
                // ✅ Allow Admin creation ONLY if explicitly enabled
                bool allowAdmin = _configuration.GetValue<bool>("AllowAdminRegistration", true);
                if (!allowAdmin)
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = "Admin registration is disabled"
                    };
                }

                roleToAssign = "Admin";
            }
            else if (registerDTO.Role?.Equals("Employee", StringComparison.OrdinalIgnoreCase) == true)
            {
                roleToAssign = "Employee";
            }
            else
            {
                roleToAssign = "User"; // Default visitor role
            }

            // 4️⃣ Assign role claim
            await _userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Role, roleToAssign));

            int? employeeId = null;

            // 5️⃣ If Employee, create Employee record
            if (roleToAssign == "Employee")
            {
                var existingEmployee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Email == registerDTO.Email);

                if (existingEmployee == null)
                {
                    var employee = new Employee
                    {
                        FullName = registerDTO.FullName,
                        Email = registerDTO.Email
                    };

                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();

                    employeeId = employee.Id;
                }
                else
                {
                    employeeId = existingEmployee.Id;
                }
            }

            // 6️⃣ Generate JWT token
            var token = await GenerateJwtToken(newUser);

            // 7️⃣ Return response
            return new AuthResponseDTO
            {
                Success = true,
                Message = "Registration successful",
                Email = newUser.Email,
                FullName = newUser.FullName,
                UserId = newUser.Id,
                Role = roleToAssign,
                EmployeeId = employeeId,
                Token = token
            };
        }

        // ✅ Login user
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = "Invalid credentials"
                };
            }

            var token = await GenerateJwtToken(user);

            // Get role from claims
            var roleClaim = (await _userManager.GetClaimsAsync(user))
                                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // ✅ Get EmployeeId if applicable
            int? employeeId = null;
            if (!string.IsNullOrEmpty(roleClaim) &&
                roleClaim.Equals("Employee", StringComparison.OrdinalIgnoreCase))
            {
                var emp = await _context.Employees.FirstOrDefaultAsync(e => e.Email == user.Email);
                if (emp != null)
                    employeeId = emp.Id;
            }

            return new AuthResponseDTO
            {
                Success = true,
                Token = token,
                Message = "Login successful",
                Email = user.Email,
                FullName = user.FullName,
                UserId = user.Id,
                Role = roleClaim,
                EmployeeId = employeeId
            };
        }

        // ✅ Helper: Get EmployeeId by Email
        public async Task<int?> GetEmployeeIdByEmailAsync(string email)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
            return employee?.Id;
        }

        // ✅ JWT token generation
        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FullName", user.FullName)
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roleClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!string.IsNullOrEmpty(roleClaim))
            {
                claims.Add(new Claim(ClaimTypes.Role, roleClaim));
            }

            var jwtConfig = new JwtConfig();
            _configuration.GetSection("JwtConfig").Bind(jwtConfig);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtConfig.Issuer,
                audience: jwtConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtConfig.DurationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            // Optional implementation
            return new List<UserDTO>();
        }
    }
}
