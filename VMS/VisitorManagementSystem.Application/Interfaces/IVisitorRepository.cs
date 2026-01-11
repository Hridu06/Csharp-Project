using VisitorManagementSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisitorManagementSystem.Application.Interfaces
{
    public interface IVisitorRepository
    {
        Task<List<Visitor>> GetAllAsync();
        Task<Visitor> GetByIdAsync(int id);
        Task<int> AddAsync(Visitor visitor);
        Task<int> UpdateAsync(Visitor visitor);
        Task<int> DeleteAsync(int id);
    }
}
