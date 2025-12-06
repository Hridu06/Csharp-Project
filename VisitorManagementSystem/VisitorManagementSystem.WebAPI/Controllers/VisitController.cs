using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitorManagementSystem.Domain.Entities;
using VisitorManagementSystem.Infrastructure.Repositories;
using VisitorManagementSystem.Infrastructure.Services;

[ApiController]
[Route("api/[controller]")]
public class VisitController : ControllerBase
{
    private readonly VisitRequestRepository _visitRepo;
    private readonly VisitorApprovalService _approvalService;
    private readonly NotificationService _notificationService;

    public VisitController(
        VisitRequestRepository visitRepo,
        VisitorApprovalService approvalService,
        NotificationService notificationService)
    {
        _visitRepo = visitRepo;
        _approvalService = approvalService;
        _notificationService = notificationService;
    }

    // ✅ Visitor requests a visit
    [HttpPost("request")]
    [Authorize(Roles = "Visitor")]
    public async Task<IActionResult> RequestVisit(VisitRequest request)
    {
        var id = await _visitRepo.AddAsync(request);
        await _notificationService.SendVisitorRequestNotificationAsync(request.EmployeeId.ToString(), "VisitorName");
        return Ok(new { id });
    }

    // ✅ Employee approves
    [HttpPost("approve/{id}")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Approve(int id, string visitorId)
    {
        await _approvalService.ApproveRequestAsync(id, visitorId);
        return Ok();
    }

    // ✅ Employee declines
    [HttpPost("decline/{id}")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Decline(int id, string visitorId)
    {
        await _approvalService.DeclineRequestAsync(id, visitorId);
        return Ok();
    }
}
