using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VisitorManagementSystem.Blazor.Models;
using Microsoft.JSInterop;

namespace VisitorManagementSystem.Blazor.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        public async Task<AuthResultModel> RegisterAsync(RegisterModel model)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", model);
            var result = await response.Content.ReadFromJsonAsync<AuthResultModel>();

            if (result != null && result.Success && !string.IsNullOrEmpty(result.Token))
            {
                await SaveUserToLocalStorage(result);
            }

            return result!;
        }

        public async Task<AuthResultModel> LoginAsync(LoginModel model)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", model);
            var result = await response.Content.ReadFromJsonAsync<AuthResultModel>();

            if (result != null && result.Success && !string.IsNullOrEmpty(result.Token))
            {
                await SaveUserToLocalStorage(result);
            }

            return result!;
        }

        private async Task SaveUserToLocalStorage(AuthResultModel result)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", "authToken", result.Token);
            await _js.InvokeVoidAsync("localStorage.setItem", "userRole", result.Role ?? string.Empty);
            await _js.InvokeVoidAsync("localStorage.setItem", "userEmail", result.Email ?? string.Empty);
            await _js.InvokeVoidAsync("localStorage.setItem", "userId", result.UserId ?? string.Empty);
            await _js.InvokeVoidAsync("localStorage.setItem", "userFullName", result.FullName ?? string.Empty);

            if (result.EmployeeId.HasValue)
            {
                await _js.InvokeVoidAsync("localStorage.setItem", "userEmployeeId", result.EmployeeId.Value.ToString());
            }
        }

        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
            await _js.InvokeVoidAsync("localStorage.removeItem", "userRole");
            await _js.InvokeVoidAsync("localStorage.removeItem", "userEmail");
            await _js.InvokeVoidAsync("localStorage.removeItem", "userId");
            await _js.InvokeVoidAsync("localStorage.removeItem", "userFullName");
            await _js.InvokeVoidAsync("localStorage.removeItem", "userEmployeeId");
        }

        // ✅ Implement the missing interface member
        public async Task<int?> GetEmployeeIdByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            try
            {
                // Call your API endpoint to fetch EmployeeId by email
                var empId = await _http.GetFromJsonAsync<int?>($"api/employees/getIdByEmail?email={email}");
                return empId;
            }
            catch
            {
                return null;
            }
        }
    }
}
