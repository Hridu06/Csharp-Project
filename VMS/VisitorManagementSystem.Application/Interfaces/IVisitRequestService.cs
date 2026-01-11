using VisitorManagementSystem.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisitorManagementSystem.Application.Interfaces
{
    public interface IVisitRequestService
    {
        Task<List<VisitRequestDto>> GetRequestsForEmployeeAsync(int employeeId);
        Task<VisitRequestDto?> CreateVisitRequestAsync(VisitRequestDto request);
        Task<bool> ApproveRequest(int requestId);
        Task<bool> RejectRequest(int requestId);
    }
}
