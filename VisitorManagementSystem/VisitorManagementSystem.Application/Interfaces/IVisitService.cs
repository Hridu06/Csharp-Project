using VisitorManagementSystem.Application.DTOs;

namespace VisitorManagementSystem.Application.Interfaces
{
    public interface IVisitService
    {
        Task<VisitRequestDto> CreateVisitRequestAsync(int visitorId, int employeeId, string reason);
        Task ApproveVisitRequestAsync(int visitRequestId, int employeeId);
        Task<IEnumerable<VisitRequestDto>> GetVisitsForEmployeeAsync(int employeeId);
        Task<IEnumerable<VisitRequestDto>> GetVisitsForVisitorAsync(int visitorId);
    }
}
