using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem_API.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
