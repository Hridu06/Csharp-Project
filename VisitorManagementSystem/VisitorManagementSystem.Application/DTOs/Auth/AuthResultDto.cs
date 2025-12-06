namespace VisitorManagementSystem.Application.DTOs.Auth
{
    public class AuthResultDto
    {
        public bool Success { get; set; }
        public string? Token { get; set; }      // JWT token
        public string? ErrorMessage { get; set; }
    }
}
