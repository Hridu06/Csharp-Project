using System.ComponentModel.DataAnnotations;

namespace BMS.Models
{
    public class BookShop
    {
        [Key]
        public int BookShopId { get; set; } 


        
        public int BookId { get; set; }
        public int ShopId { get; set; }


        
        public Book? Book { get; set; }
        public Shop? Shop { get; set; }


        
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }


        
        public string Display => $"{Book?.Title} @ {Shop?.Name}";
    }
}
