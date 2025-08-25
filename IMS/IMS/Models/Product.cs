using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = default!;

        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "5000.00",
            ErrorMessage = "UnitPrice must be between 0.01 and 5000.00.")]
        public decimal UnitPrice { get; set; }

        [StringLength(50)]
        public string? Category { get; set; }

        [StringLength(20)]
        public string? SKU { get; set; }

        // Navigation
        [InverseProperty(nameof(ProductWarehouse.Product))]
        public ICollection<ProductWarehouse> ProductWarehouses { get; set; } = new List<ProductWarehouse>();

        [InverseProperty(nameof(Shipment.Product))]
        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
