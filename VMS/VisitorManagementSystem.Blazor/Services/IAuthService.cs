using VisitorManagementSystem.Blazor.Models;
using System.Threading.Tasks;

namespace VisitorManagementSystem.Blazor.Services
{
    public interface IAuthService
    {
        Task<AuthResultModel> LoginAsync(LoginModel loginModel);
        Task<AuthResultModel> RegisterAsync(RegisterModel registerModel);
        Task<int?> GetEmployeeIdByEmailAsync(string email);

    }
}
