using VisitorManagementSystem.Application.DTOs;

namespace VisitorManagementSystem.Application.Interfaces
{
    public interface INotificationService
    {
        Task NotifyEmployeeVisitRequestAsync(int employeeId, VisitRequestDto visitRequest);
    }
}
