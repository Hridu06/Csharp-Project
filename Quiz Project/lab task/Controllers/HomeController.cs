using System.Diagnostics;
using lab_task.Models;
using Microsoft.AspNetCore.Mvc;

namespace lab_task.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("info")]
        public IActionResult About()
        {

            return View();
        }
    }
}
