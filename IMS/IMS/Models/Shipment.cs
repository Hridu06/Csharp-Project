using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models
{
    public class Shipment
    {
        [Key]
        public int ShipmentId { get; set; }

        // FKs
        [Required]
        public int SupplierId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        public DateTime ShipmentDate { get; set; }

        // Persisted total cost (can also be computed in DB if you choose to configure it as a computed column)
        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335",
            ErrorMessage = "TotalCost cannot be negative.")]
        public decimal TotalCost { get; set; }

        // Navigation
        [ForeignKey(nameof(SupplierId))]
        [InverseProperty(nameof(Models.Supplier.Shipments))]
        public Supplier Supplier { get; set; } = default!;

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(Models.Product.Shipments))]
        public Product Product { get; set; } = default!;

        [ForeignKey(nameof(WarehouseId))]
        [InverseProperty(nameof(Models.Warehouse.Shipments))]
        public Warehouse Warehouse { get; set; } = default!;

        // Optional helper (not mapped) if you want to verify/compute on the fly in code
        [NotMapped]
        public decimal ComputedTotalCost => Quantity * (Product?.UnitPrice ?? 0m);
    }
}
