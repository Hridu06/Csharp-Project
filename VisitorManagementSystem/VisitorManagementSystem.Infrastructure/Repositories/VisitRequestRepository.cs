using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisitorManagementSystem.Domain.Entities;
using VisitorManagementSystem.Domain.Enums;
using VisitorManagementSystem.Infrastructure.Data;

namespace VisitorManagementSystem.Infrastructure.Repositories
{
    public class VisitRequestRepository
    {
        private readonly DapperContext _context;

        public VisitRequestRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VisitRequest>> GetAllAsync()
        {
            var query = "SELECT * FROM VisitRequests";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<VisitRequest>(query);
            }
        }

        public async Task<VisitRequest> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM VisitRequests WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<VisitRequest>(query, new { Id = id });
            }
        }

        public async Task<int> AddAsync(VisitRequest request)
        {
            var query = @"INSERT INTO VisitRequests (VisitorId, EmployeeId, RequestDate, Status)
                          VALUES (@VisitorId, @EmployeeId, @RequestDate, @Status);
                          SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, request);
                return id;
            }
        }

        public async Task<int> UpdateStatusAsync(int requestId, VisitStatus status)
        {
            var query = @"UPDATE VisitRequests SET Status = @Status WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { Status = status.ToString(), Id = requestId });
            }
        }

        public async Task<IEnumerable<VisitRequest>> GetPendingRequestsByEmployeeIdAsync(int employeeId)
        {
            var query = @"SELECT * FROM VisitRequests WHERE EmployeeId = @EmployeeId AND Status = 'Pending'";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<VisitRequest>(query, new { EmployeeId = employeeId });
            }
        }
    }
}
