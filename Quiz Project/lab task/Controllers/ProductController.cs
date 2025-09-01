using lab_task.Models;
using Microsoft.AspNetCore.Mvc;

namespace lab_task.Controllers
{
    public class ProductController : Controller
    {
        private static List<Product> products = new List<Product>
        {
            new Product{ Id = 1, Name = "Laptop", Price = 800 },
            new Product{ Id = 2, Name = "Mobile", Price = 400 },
            new Product{ Id = 3, Name = "Headphones", Price = 50 }
        };

        public ActionResult List()
        {
            return View(products);
        }

        public ActionResult Order(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            ViewBag.ProductName = product?.Name;
            return View();
        }
    }
}
