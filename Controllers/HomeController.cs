using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.Controllers
{
    public class HomeController : Controller
    {
        // Optional: protect dashboard
        public IActionResult Index()
        {
            return View(); // Views/Home/Index.cshtml
        }
    }
}