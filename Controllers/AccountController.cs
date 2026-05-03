using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Views/Account/Login.cshtml
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(); // Views/Account/Register.cshtml
        }
    }
}   