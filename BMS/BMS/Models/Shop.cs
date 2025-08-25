using System.ComponentModel.DataAnnotations;

namespace BMS.Models
{
    public class Shop
    {
        [Key]
        public int ShopId { get; set; }


        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;


        [Required, StringLength(200)]
        public string Location { get; set; } = string.Empty;


        [StringLength(30)]
        public string? Phone { get; set; }


        public ICollection<BookShop> BookShops { get; set; } = new List<BookShop>();
    }
}
