using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitorManagementSystem.Application.DTOs;
using VisitorManagementSystem.Application.Interfaces;

namespace VisitorManagementSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // ✅ Require authentication for all endpoints
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // ✅ GET: api/Employees
        // Admin, Employee, and User can view the list
        [HttpGet]
        [Authorize(Roles = "Admin,Employee,User")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var employees = await _employeeService.GetAllAsync();
            return Ok(employees);
        }

        // ✅ GET: api/Employees/{id}
        // Admin, Employee, and User can view employee details
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Employee,User")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
                return NotFound("Employee not found.");

            return Ok(employee);
        }

        // ✅ POST: api/Employees
        // Only Admin can add new employees
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid employee data.");

            var success = await _employeeService.AddAsync(dto);
            if (!success)
                return StatusCode(500, "Failed to add employee.");

            return Ok("Employee added successfully.");
        }

        // ✅ PUT: api/Employees/{id}
        // Only Admin can update employee info
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDto dto)
        {
            if (dto == null || id != dto.Id)
                return BadRequest("Invalid employee data.");

            var success = await _employeeService.UpdateAsync(dto);
            if (!success)
                return NotFound("Employee not found or update failed.");

            return Ok("Employee updated successfully.");
        }

        // ✅ DELETE: api/Employees/{id}
        // Only Admin can delete employees
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var success = await _employeeService.DeleteAsync(id);
            if (!success)
                return NotFound("Employee not found or deletion failed.");

            return Ok("Employee deleted successfully.");
        }
    }
}
