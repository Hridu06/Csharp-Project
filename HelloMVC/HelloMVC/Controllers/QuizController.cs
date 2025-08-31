using Microsoft.AspNetCore.Mvc;

namespace HelloMVC.Controllers
{
    public class QuizController : Controller
    {
        
        [HttpGet]
        public IActionResult Question()
        {
            ViewBag.Question = "What is the capital of France?";
            ViewBag.Options = new List<string> { "Paris", "London", "Berlin" };
            return View();
        }

       
        [HttpPost]
        public IActionResult Question(string selectedOption)
        {
            string correctAnswer = "Paris";
            ViewBag.Result = selectedOption == correctAnswer ? "Correct ✅" : "Incorrect ❌";
            return View("Result");
        }

        
        public IActionResult Result()
        {
            return View();
        }
    }
}
