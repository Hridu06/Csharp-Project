using Dapper;
using System.Data;
using VisitorManagementSystem.Application.DTOs;
using VisitorManagementSystem.Application.Interfaces;
using VisitorManagementSystem.Domain.Entities;

namespace VisitorManagementSystem.Infrastructure.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly IDbConnection _dbConnection;

        public VisitorService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // ✅ Add Visitor
        public async Task<VisitorDTO> AddVisitorAsync(VisitorDTO visitorDto)
        {
            string query = @"INSERT INTO Visitors (FullName, Email, Phone, Address, VisitDate, Purpose)
                             VALUES (@FullName, @Email, @Phone, @Address, @VisitDate, @Purpose);
                             SELECT CAST(SCOPE_IDENTITY() as int);";

            var id = await _dbConnection.ExecuteScalarAsync<int>(query, visitorDto);
            visitorDto.Id = id;
            return visitorDto;
        }

        // ✅ Get All Visitors
        public async Task<List<VisitorDTO>> GetAllVisitorsAsync()
        {
            string query = "SELECT * FROM Visitors";
            var visitors = await _dbConnection.QueryAsync<VisitorDTO>(query);
            return visitors.ToList();
        }

        // ✅ Get Visitor by Id
        public async Task<VisitorDTO> GetVisitorByIdAsync(int id)
        {
            string query = "SELECT * FROM Visitors WHERE Id = @Id";
            var visitor = await _dbConnection.QueryFirstOrDefaultAsync<VisitorDTO>(query, new { Id = id });
            return visitor;
        }

        // ✅ Update Visitor
        public async Task<bool> UpdateVisitorAsync(VisitorDTO visitorDto)
        {
            string query = @"UPDATE Visitors 
                             SET FullName = @FullName, 
                                 Email = @Email, 
                                 Phone = @Phone, 
                                 Address = @Address,
                                 VisitDate = @VisitDate,
                                 Purpose = @Purpose
                             WHERE Id = @Id";

            var rowsAffected = await _dbConnection.ExecuteAsync(query, visitorDto);
            return rowsAffected > 0;
        }

        // ✅ Delete Visitor
        public async Task<bool> DeleteVisitorAsync(int id)
        {
            string query = "DELETE FROM Visitors WHERE Id = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }


    }
}
