using Microsoft.AspNetCore.Mvc;

namespace HelloMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            ViewBag.Name = "Sabbir Hossain Hridoy";
            ViewBag.Age = 24;
            ViewBag.Country = "Bangladesh";
            
            
            return View();
        }
    }
}
