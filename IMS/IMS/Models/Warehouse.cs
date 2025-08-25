using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models
{
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = default!;

        [Required, StringLength(100)]
        public string Location { get; set; } = default!;

        [Range(0, int.MaxValue, ErrorMessage = "StorageCapacity cannot be negative.")]
        public int StorageCapacity { get; set; }

        // Navigation
        [InverseProperty(nameof(ProductWarehouse.Warehouse))]
        public ICollection<ProductWarehouse> ProductWarehouses { get; set; } = new List<ProductWarehouse>();

        [InverseProperty(nameof(Shipment.Warehouse))]
        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
