namespace VisitorManagementSystem.Domain.Entities
{
    public class Visitor
    {
        public int Id { get; set; }
        public string? FullName { get; set; }    // ✅ Must match
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime VisitDate { get; set; }
        public string? Purpose { get; set; }
    }
}
