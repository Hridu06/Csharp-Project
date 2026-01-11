using System.Collections.Generic;
using System.Threading.Tasks;
using VisitorManagementSystem.Application.DTOs;
using VisitorManagementSystem.Domain.Entities;

namespace VisitorManagementSystem.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<bool> AddAsync(EmployeeDto employee);
        Task<bool> UpdateAsync(EmployeeDto employee);
        Task<bool> DeleteAsync(int id);

        // ✅ New methods needed by AuthService
        Task AddEmployeeAsync(Employee employee);           // For creating directly from AuthService
        Task<Employee?> GetEmployeeByEmailAsync(string email); // For finding employee by email
    }
}
