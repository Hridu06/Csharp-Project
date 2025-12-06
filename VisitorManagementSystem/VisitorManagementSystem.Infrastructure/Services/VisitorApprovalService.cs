using System.Threading.Tasks;
using VisitorManagementSystem.Domain.Enums;
using VisitorManagementSystem.Infrastructure.Repositories;

namespace VisitorManagementSystem.Infrastructure.Services
{
    public class VisitorApprovalService
    {
        private readonly VisitRequestRepository _visitRequestRepository;
        private readonly NotificationService _notificationService;

        public VisitorApprovalService(
            VisitRequestRepository visitRequestRepository,
            NotificationService notificationService)
        {
            _visitRequestRepository = visitRequestRepository;
            _notificationService = notificationService;
        }

        // Approve a visitor request
        public async Task ApproveRequestAsync(int requestId, string visitorId)
        {
            await _visitRequestRepository.UpdateStatusAsync(requestId, VisitStatus.Approved);
            await _notificationService.SendVisitorRequestStatusAsync(visitorId, VisitStatus.Approved.ToString());
        }

        // Decline a visitor request
        public async Task DeclineRequestAsync(int requestId, string visitorId)
        {
            await _visitRequestRepository.UpdateStatusAsync(requestId, VisitStatus.Declined);
            await _notificationService.SendVisitorRequestStatusAsync(visitorId, VisitStatus.Declined.ToString());
        }
    }
}
