using HelloMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    public class ProductController : Controller
    {
        // Static list shared by all actions
        private static List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1500 },
            new Product { Id = 2, Name = "Smartphone", Price = 800 },
            new Product { Id = 3, Name = "Headphones", Price = 150 }
        };

        // GET: /Product/List
        public IActionResult List()
        {
            return View(products);
        }

        // GET: /Product/Order/1
        public IActionResult Order(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return Content("Product not found!");

            ViewBag.ProductName = product.Name;
            ViewBag.ProductPrice = product.Price;
            return View();
        }
    }
}
