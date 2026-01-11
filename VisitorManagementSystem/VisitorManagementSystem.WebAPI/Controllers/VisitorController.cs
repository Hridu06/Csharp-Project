using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using VisitorManagementSystem.Application.DTOs;
using VisitorManagementSystem.Application.Interfaces;
using VisitorManagementSystem.WebAPI.Hubs;

namespace VisitorManagementSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // All endpoints require authentication
    public class VisitorController : ControllerBase
    {
        private readonly IVisitorService _visitorService;
        private readonly IHubContext<VisitorHub> _hubContext;

        public VisitorController(IVisitorService visitorService, IHubContext<VisitorHub> hubContext)
        {
            _visitorService = visitorService;
            _hubContext = hubContext;
        }

        // ✅ GET: api/Visitor
        // Both Admin and User (Visitor) can view visitor list
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllVisitors()
        {
            var visitors = await _visitorService.GetAllVisitorsAsync();
            return Ok(visitors);
        }

        // ✅ GET: api/Visitor/{id}
        // Both Admin and User (Visitor) can view visitor details
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetVisitorById(int id)
        {
            var visitor = await _visitorService.GetVisitorByIdAsync(id);
            if (visitor == null)
                return NotFound("Visitor not found.");

            return Ok(visitor);
        }

        // ❌ Only Admin can create visitors
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddVisitor([FromBody] VisitorDTO visitorDto)
        {
            if (visitorDto == null)
                return BadRequest("Invalid visitor data.");

            var result = await _visitorService.AddVisitorAsync(visitorDto);
            if (result != null)
            {
                // 🔔 Notify all clients using SignalR
                await _hubContext.Clients.All.SendAsync("ReceiveVisitorNotification", visitorDto.FullName);
                return Ok(result);
            }

            return StatusCode(500, "Failed to add visitor.");
        }

        // ❌ Only Admin can update visitors
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVisitor(int id, [FromBody] VisitorDTO visitorDto)
        {
            if (visitorDto == null || id != visitorDto.Id)
                return BadRequest("Invalid visitor data.");

            var updated = await _visitorService.UpdateVisitorAsync(visitorDto);
            if (updated)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveVisitorUpdate", visitorDto.FullName);
                return Ok("Visitor updated successfully.");
            }

            return NotFound("Visitor not found.");
        }

        // ❌ Only Admin can delete visitors
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVisitor(int id)
        {
            var deleted = await _visitorService.DeleteVisitorAsync(id);
            if (deleted)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveVisitorDelete", id);
                return Ok("Visitor deleted successfully.");
            }

            return NotFound("Visitor not found.");
        }
    }
}
