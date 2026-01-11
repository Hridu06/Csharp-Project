using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using VisitorManagementSystem.Application.Interfaces;
using VisitorManagementSystem.Application.DTOs;
using VisitorManagementSystem.WebAPI.Hubs;
using System.Security.Claims;

namespace VisitorManagementSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication globally
    public class VisitRequestsController : ControllerBase
    {
        private readonly IVisitRequestService _visitRequestService;
        private readonly IEmployeeService _employeeService;
        private readonly IHubContext<VisitorHub> _visitorHub;

        public VisitRequestsController(
            IVisitRequestService visitRequestService,
            IEmployeeService employeeService,
            IHubContext<VisitorHub> visitorHub)
        {
            _visitRequestService = visitRequestService;
            _employeeService = employeeService;
            _visitorHub = visitorHub;
        }

        // ✅ Visitors can create a visit request
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<VisitRequestDto>> CreateVisitRequest([FromBody] VisitRequestDto requestDto)
        {
            if (requestDto == null)
                return BadRequest("Invalid visit request data.");

            // Get VisitorId from authenticated AspNetUser
            var visitorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(visitorIdString))
                return Unauthorized("Invalid or missing Visitor ID.");

            // Attach visitor info
            requestDto.VisitorId = visitorIdString;
            requestDto.CreatedAt = DateTime.UtcNow;

            try
            {
                // Validate employee exists
                var employee = await _employeeService.GetByIdAsync(requestDto.EmployeeId);
                if (employee == null)
                    return NotFound($"Employee with ID {requestDto.EmployeeId} not found.");

                requestDto.EmployeeName = employee.FullName;

                var createdRequest = await _visitRequestService.CreateVisitRequestAsync(requestDto);
                if (createdRequest == null)
                    return StatusCode(500, "Failed to create visit request.");

                // Notify the employee via SignalR
                await _visitorHub.Clients
                    .Group($"Employee_{employee.Id}")
                    .SendAsync("ReceiveVisitorNotification", requestDto.VisitorName, requestDto.EmployeeName);

                return Ok(createdRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        // ✅ Employee can see all visit requests addressed to them
        [HttpGet("employee/{employeeId:int}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<ActionResult<List<VisitRequestDto>>> GetRequestsForEmployee(int employeeId)
        {
            try
            {
                var requests = await _visitRequestService.GetRequestsForEmployeeAsync(employeeId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        // ✅ Employee can approve a visit request
        [HttpPut("approve/{requestId}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            var success = await _visitRequestService.ApproveRequest(requestId);
            if (!success)
                return NotFound("Request not found or could not be approved.");

            return Ok("Request approved successfully.");
        }

        // ✅ Employee can reject a visit request
        [HttpPut("reject/{requestId}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            var success = await _visitRequestService.RejectRequest(requestId);
            if (!success)
                return NotFound("Request not found or could not be rejected.");

            return Ok("Request rejected successfully.");
        }
    }
}
