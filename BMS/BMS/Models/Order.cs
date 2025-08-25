using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMS.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }


        // FKs
        public int CustomerId { get; set; }
        public int BookShopId { get; set; } // points to junction table PK


        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;


        [Range(0, 1_000_000)]
        [Column(TypeName = "decimal(12,2)")]
        public decimal TotalPrice { get; set; }


        // navigation
        public Customer? Customer { get; set; }
        public BookShop? BookShop { get; set; }
    }
}
