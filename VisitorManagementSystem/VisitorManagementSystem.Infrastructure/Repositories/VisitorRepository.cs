using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisitorManagementSystem.Domain.Entities;
using VisitorManagementSystem.Infrastructure.Data;

namespace VisitorManagementSystem.Infrastructure.Repositories
{
    public class VisitorRepository
    {
        private readonly DapperContext _context;

        public VisitorRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Visitor>> GetAllAsync()
        {
            var query = "SELECT * FROM Visitors";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Visitor>(query);
            }
        }

        public async Task<Visitor> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Visitors WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Visitor>(query, new { Id = id });
            }
        }

        public async Task<int> AddAsync(Visitor visitor)
        {
            var query = @"INSERT INTO Visitors (FullName, Email, ContactNumber, Address)
                          VALUES (@FullName, @Email, @ContactNumber, @Address);
                          SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, visitor);
                return id;
            }
        }

        public async Task<int> UpdateAsync(Visitor visitor)
        {
            var query = @"UPDATE Visitors SET 
                            FullName = @FullName,
                            Email = @Email,
                            ContactNumber = @ContactNumber,
                            Address = @Address
                          WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, visitor);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM Visitors WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
