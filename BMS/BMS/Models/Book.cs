using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMS.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }


        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;


        [Required, StringLength(120)]
        public string Author { get; set; } = string.Empty;


        [StringLength(20)]
        public string? ISBN { get; set; }


        [Range(0, 1_000_000)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }


        [DataType(DataType.Date)]
        public DateTime? PublishedOn { get; set; }


        [StringLength(1000)]
        public string? Description { get; set; }


        public ICollection<BookShop> BookShops { get; set; } = new List<BookShop>();
    }
}
