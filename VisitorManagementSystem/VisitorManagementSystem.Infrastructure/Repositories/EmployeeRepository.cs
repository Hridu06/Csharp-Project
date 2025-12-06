using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisitorManagementSystem.Domain.Entities;
using VisitorManagementSystem.Infrastructure.Data;

namespace VisitorManagementSystem.Infrastructure.Repositories
{
    public class EmployeeRepository
    {
        private readonly DapperContext _context;

        public EmployeeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var query = "SELECT * FROM Employees";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Employee>(query);
            }
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Employees WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Employee>(query, new { Id = id });
            }
        }

        public async Task<int> AddAsync(Employee employee)
        {
            var query = @"INSERT INTO Employees (FullName, Email, Department, Position)
                          VALUES (@FullName, @Email, @Department, @Position);
                          SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, employee);
                return id;
            }
        }

        public async Task<int> UpdateAsync(Employee employee)
        {
            var query = @"UPDATE Employees SET 
                            FullName = @FullName,
                            Email = @Email,
                            Department = @Department,
                            Position = @Position
                          WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, employee);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM Employees WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
