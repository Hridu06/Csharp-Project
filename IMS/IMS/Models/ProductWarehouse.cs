using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models
{
    public class ProductWarehouse
    {
        [Key]
        public int ProductWarehouseId { get; set; }

        // FKs
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        // Stock tracking
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }

        // Navigation
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(Models.Product.ProductWarehouses))]
        public Product Product { get; set; } = default!;

        [ForeignKey(nameof(WarehouseId))]
        [InverseProperty(nameof(Models.Warehouse.ProductWarehouses))]
        public Warehouse Warehouse { get; set; } = default!;
    }
}
