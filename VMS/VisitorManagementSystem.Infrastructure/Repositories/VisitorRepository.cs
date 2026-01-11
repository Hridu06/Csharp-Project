using Dapper;
using VisitorManagementSystem.Application.Interfaces;
using VisitorManagementSystem.Domain.Entities;
using VisitorManagementSystem.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisitorManagementSystem.Infrastructure.Repository
{
    public class VisitorRepository : IVisitorRepository
    {
        private readonly DapperContext _context;

        public VisitorRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<Visitor>> GetAllAsync()
        {
            var query = "SELECT * FROM Visitors ORDER BY Id ASC";
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Visitor>(query);
            return result.AsList();
        }

        public async Task<Visitor> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Visitors WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Visitor>(query, new { Id = id });
        }

        public async Task<int> AddAsync(Visitor visitor)
        {
            var query = @"INSERT INTO Visitors (FullName, Contact, Purpose, VisitDate)
                          VALUES (@FullName, @Contact, @Purpose, @VisitDate)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(query, visitor);
        }

        public async Task<int> UpdateAsync(Visitor visitor)
        {
            var query = @"UPDATE Visitors SET FullName=@FullName, Contact=@Contact, 
                          Purpose=@Purpose, VisitDate=@VisitDate WHERE Id=@Id";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(query, visitor);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM Visitors WHERE Id=@Id";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
