using Microsoft.AspNetCore.Mvc;

namespace UsersTest.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
