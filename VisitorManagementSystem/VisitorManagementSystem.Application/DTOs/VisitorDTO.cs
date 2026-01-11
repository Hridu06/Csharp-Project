namespace VisitorManagementSystem.Application.DTOs
{
    public class VisitorDTO
    {
        public int Id { get; set; }
        public string? FullName { get; set; }     // ✅ Add this
        public string? Email { get; set; }        // ✅ Optional, if used
        public string? Phone { get; set; }        // ✅ Optional, if used
        public string? Address { get; set; }      // ✅ Optional
        public DateTime VisitDate { get; set; }  // ✅ Optional
        public string? Purpose { get; set; }      // ✅ Optional
    }
}
