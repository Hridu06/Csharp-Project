using lab_task.Models;
using Microsoft.AspNetCore.Mvc;

namespace lab_task.Controllers
{
    public class StudentController : Controller
    {
       
        public ActionResult Register()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Register(Student student)
        {
            
            return View("Confirmation", student);
        }

       
        public ActionResult Calculator()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Calculate(int a, int b)
        {
            ViewBag.Sum = a + b;
            ViewBag.Difference = a - b;
            ViewBag.Product = a * b;

            return View("Result");
        }
    }
}

