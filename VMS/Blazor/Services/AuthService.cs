using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using VisitorManagementSystem.Application.DTOs;

namespace VisitorManagementSystem.Blazor.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        private const string TokenKey = "authToken";
        private const string RoleKey = "role";
        private const string FullNameKey = "fullName";
        private const string EmployeeIdKey = "employeeId";
        private const string UserIdKey = "userId";

        // ✅ Login user
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/Auth/login", dto);

            if (!response.IsSuccessStatusCode)
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = "Login failed. Server error."
                };
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();

            if (result == null)
                return new AuthResponseDTO { Success = false, Message = "Invalid server response." };

            if (result.Success)
            {
                // Store JWT, role, and other info
                await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, result.Token);
                await _js.InvokeVoidAsync("localStorage.setItem", RoleKey, result.Role ?? "");
                await _js.InvokeVoidAsync("localStorage.setItem", FullNameKey, result.FullName ?? "");

                if (result.Role == "Employee")
                    await _js.InvokeVoidAsync("localStorage.setItem", EmployeeIdKey, result.EmployeeId?.ToString() ?? "");
                else // Visitor
                    await _js.InvokeVoidAsync("localStorage.setItem", UserIdKey, result.UserId ?? "");

                SetAuthHeader(result.Token);
            }

            return result;
        }

        // ✅ Register user
        public async Task<bool> RegisterAsync(RegisterDTO register)
        {
            var response = await _http.PostAsJsonAsync("api/Auth/register", register);
            return response.IsSuccessStatusCode;
        }

        // ✅ Logout user
        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            await _js.InvokeVoidAsync("localStorage.removeItem", RoleKey);
            await _js.InvokeVoidAsync("localStorage.removeItem", FullNameKey);
            await _js.InvokeVoidAsync("localStorage.removeItem", EmployeeIdKey);
            await _js.InvokeVoidAsync("localStorage.removeItem", UserIdKey);

            _http.DefaultRequestHeaders.Authorization = null;
        }

        // ✅ Get JWT token
        public async Task<string?> GetTokenAsync()
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
        }

        // ✅ Get user role
        public async Task<string?> GetRoleAsync()
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", RoleKey);
        }

        // ✅ Check if logged in
        public async Task<bool> IsLoggedInAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }

        // ✅ Check if user is Admin
        public async Task<bool> IsAdminAsync()
        {
            var role = await GetRoleAsync();
            return role == "Admin";
        }

        // ✅ Set Authorization header manually
        public void SetAuthHeader(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // ✅ Load token from storage and attach header (on app startup)
        public async Task InitializeAsync()
        {
            var token = await GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                SetAuthHeader(token);
            }
        }
    }
}
