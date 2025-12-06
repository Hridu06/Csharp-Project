using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitorManagementSystem.Domain.Entities;
using VisitorManagementSystem.Infrastructure.Repositories;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeRepository _employeeRepo;

    public EmployeeController(EmployeeRepository employeeRepo)
    {
        _employeeRepo = employeeRepo;
    }

    // ✅ Anyone authenticated can view employees
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll() => Ok(await _employeeRepo.GetAllAsync());

    // ✅ Only Admin can add employees
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Add(Employee employee)
    {
        var id = await _employeeRepo.AddAsync(employee);
        return Ok(new { id });
    }

    // ✅ Only Admin can update
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, Employee employee)
    {
        employee.Id = id;
        await _employeeRepo.UpdateAsync(employee);
        return Ok();
    }

    // ✅ Only Admin can delete
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _employeeRepo.DeleteAsync(id);
        return Ok();
    }
}
