using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required, StringLength(50)]
        public string CompanyName { get; set; } = default!;

        [Required, EmailAddress]
        public string ContactEmail { get; set; } = default!;

        [StringLength(15)]
        [Phone] // Accepts common phone formats; length constrained by StringLength above
        public string? ContactPhone { get; set; }

        [StringLength(100)]
        public string? Address { get; set; }

        // Navigation
        [InverseProperty(nameof(Shipment.Supplier))]
        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
