using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorManagementSystem.Application.DTOs;
using VisitorManagementSystem.Application.Interfaces;
using VisitorManagementSystem.Domain.Entities;
using VisitorManagementSystem.Infrastructure.Data;

namespace VisitorManagementSystem.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeDto>> GetAllAsync()
        {
            return await _context.Employees
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    Department = e.Department,
                    Email = e.Email,
                    IsAvailable = e.IsAvailable
                }).ToListAsync();
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var e = await _context.Employees.FindAsync(id);
            if (e == null) return null;

            return new EmployeeDto
            {
                Id = e.Id,
                FullName = e.FullName,
                Department = e.Department,
                Email = e.Email,
                IsAvailable = e.IsAvailable
            };
        }

        public async Task<bool> AddAsync(EmployeeDto dto)
        {
            // prevent duplicate employees by email
            var existing = await _context.Employees.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (existing != null)
                return false;

            var employee = new Employee
            {
                FullName = dto.FullName,
                Department = dto.Department,
                Email = dto.Email,
                IsAvailable = dto.IsAvailable
            };

            _context.Employees.Add(employee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(EmployeeDto dto)
        {
            var employee = await _context.Employees.FindAsync(dto.Id);
            if (employee == null) return false;

            employee.FullName = dto.FullName;
            employee.Department = dto.Department;
            employee.Email = dto.Email;
            employee.IsAvailable = dto.IsAvailable;

            _context.Employees.Update(employee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            _context.Employees.Remove(employee);
            return await _context.SaveChangesAsync() > 0;
        }

        // ✅ Used by AuthService to add automatically when user with "Employee" role registers
        public async Task AddEmployeeAsync(Employee employee)
        {
            var existing = await _context.Employees.FirstOrDefaultAsync(e => e.Email == employee.Email);
            if (existing != null)
                return; // prevent duplicate

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        // ✅ Used by AuthService or VisitRequestService to get employee by email
        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}
