using System.Net.Http.Json;
using VisitorManagementSystem.Application.DTOs;
using VisitorManagementSystem.Application.Interfaces;

namespace VisitorManagementSystem.Blazor.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ✅ Get all employees
        public async Task<List<EmployeeDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<EmployeeDto>>("api/employees") ?? new List<EmployeeDto>();
        }

        // ✅ Get employee by Id
        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<EmployeeDto>($"api/employees/{id}");
        }

        // ✅ Add new employee
        public async Task<bool> AddAsync(EmployeeDto employee)
        {
            var response = await _httpClient.PostAsJsonAsync("api/employees", employee);
            return response.IsSuccessStatusCode;
        }

        // ✅ Update employee
        public async Task<bool> UpdateAsync(EmployeeDto employee)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/employees/{employee.Id}", employee);
            return response.IsSuccessStatusCode;
        }

        // ✅ Delete employee
        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/employees/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
