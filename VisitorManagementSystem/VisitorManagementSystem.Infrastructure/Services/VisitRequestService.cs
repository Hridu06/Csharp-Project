using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VisitorManagementSystem.Application.DTOs;
using VisitorManagementSystem.Application.Interfaces;
using VisitorManagementSystem.Domain.Entities;
using VisitorManagementSystem.Infrastructure.Data;

namespace VisitorManagementSystem.Infrastructure.Services
{
    public class VisitRequestService : IVisitRequestService
    {
        private readonly ApplicationDbContext _context;

        public VisitRequestService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Get all requests for a specific employee (int EmployeeId)
        public async Task<List<VisitRequestDto>> GetRequestsForEmployeeAsync(int employeeId)
        {
            return await _context.VisitRequests
                .Where(r => r.EmployeeId == employeeId)
                .Select(r => new VisitRequestDto
                {
                    Id = r.Id,
                    VisitorId = r.VisitorId,
                    EmployeeId = r.EmployeeId,
                    VisitorName = _context.Users
                        .Where(u => u.Id == r.VisitorId)
                        .Select(u => u.FullName)
                        .FirstOrDefault() ?? "Unknown Visitor",
                    EmployeeName = _context.Employees
                        .Where(e => e.Id == r.EmployeeId)
                        .Select(e => e.FullName)
                        .FirstOrDefault() ?? "Unknown Employee",
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        // ✅ Create a new visit request
        public async Task<VisitRequestDto?> CreateVisitRequestAsync(VisitRequestDto request)
        {
            if (request == null)
                return null;

            // Check if employee exists
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == request.EmployeeId);
            if (employee == null)
                throw new Exception($"Employee with ID {request.EmployeeId} not found.");

            // Create entity
            var entity = new VisitRequest
            {
                VisitorId = request.VisitorId,   // string
                EmployeeId = request.EmployeeId, // int
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.VisitRequests.Add(entity);
            await _context.SaveChangesAsync();

            // Get visitor details
            var visitor = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.VisitorId);

            return new VisitRequestDto
            {
                Id = entity.Id,
                VisitorId = entity.VisitorId,
                EmployeeId = entity.EmployeeId,
                VisitorName = visitor?.FullName ?? "Unknown Visitor",
                EmployeeName = employee.FullName,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt
            };
        }

        // ✅ Approve a visit request
        public async Task<bool> ApproveRequest(int requestId)
        {
            var entity = await _context.VisitRequests.FindAsync(requestId);
            if (entity == null)
                return false;

            entity.Status = "Approved";
            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ Reject a visit request
        public async Task<bool> RejectRequest(int requestId)
        {
            var entity = await _context.VisitRequests.FindAsync(requestId);
            if (entity == null)
                return false;

            entity.Status = "Rejected";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
