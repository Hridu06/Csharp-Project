using HelloMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelloMVC.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            Student s1 = new Student();

            s1.Id = 22103119;
            s1.Name = "Sabbir Hossain Hridoy";
            s1.Email = "sabbir@gmail.com";
            s1.Grade = 80;
            
            return View(s1);
        }

        public IActionResult StudentList()
        {
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "Sabbir", Email = "sabbir@example.com", Grade = 85 },
                new Student { Id = 2, Name = "Alvee", Email = "alvee@example.com", Grade = 90 },
                new Student { Id = 3, Name = "Mahia", Email = "mahia@example.com", Grade = 100 },
                new Student { Id = 4, Name = "Hridoy", Email = "hridoy@example.com", Grade = 40 }
            };
            return View(students);
        }
    }
}
