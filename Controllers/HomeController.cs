using Microsoft.AspNetCore.Mvc;

namespace EventHorizon.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}