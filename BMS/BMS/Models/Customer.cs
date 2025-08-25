using System.ComponentModel.DataAnnotations;

namespace BMS.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }


        [Required, StringLength(150)]
        public string FullName { get; set; } = string.Empty;


        [Required, EmailAddress, StringLength(200)]
        public string Email { get; set; } = string.Empty;


        [StringLength(30)]
        public string? Phone { get; set; }


        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
