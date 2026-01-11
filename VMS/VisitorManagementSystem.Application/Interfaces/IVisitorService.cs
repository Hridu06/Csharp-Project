using VisitorManagementSystem.Application.DTOs;

namespace VisitorManagementSystem.Application.Interfaces
{
    public interface IVisitorService
    {
        Task<VisitorDTO> AddVisitorAsync(VisitorDTO visitorDto);
        Task<List<VisitorDTO>> GetAllVisitorsAsync();
        Task<VisitorDTO> GetVisitorByIdAsync(int id);
        Task<bool> UpdateVisitorAsync(VisitorDTO visitorDto);   
        Task<bool> DeleteVisitorAsync(int id);
        
    }
}
