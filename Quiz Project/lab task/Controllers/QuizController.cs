using Microsoft.AspNetCore.Mvc;

namespace lab_task.Controllers
{
    public class QuizController : Controller
    {
        [HttpGet]
        public ActionResult Question()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Question(string answer)
        {
            string result = (answer == "C#") ? "Correct" : "Incorrect";
            ViewBag.Result = result;
            return View("Result");
        }
    }
}
