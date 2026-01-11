namespace VisitorManagementSystem.Blazor.Models
{
    public class VisitorModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime VisitDate { get; set; }
        public string? Purpose { get; set; }
    }
}
