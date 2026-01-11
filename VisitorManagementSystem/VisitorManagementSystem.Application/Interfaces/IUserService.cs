using VisitorManagementSystem.Application.DTOs;

namespace VisitorManagementSystem.Application.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDTO);
        Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO);
        Task<List<UserDTO>> GetAllUsersAsync();

        // ✅ Add this method
        Task<int?> GetEmployeeIdByEmailAsync(string email);
    }
}
