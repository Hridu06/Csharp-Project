using VisitorManagementSystem.Application.DTOs;

namespace VisitorManagementSystem.Blazor.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<bool> AddAsync(EmployeeDto employee);
        Task<bool> UpdateAsync(EmployeeDto employee);
        Task<bool> DeleteAsync(int id);
    }
}
