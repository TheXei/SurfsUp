using Microsoft.AspNetCore.Mvc;

namespace SurfsUp.Controllers
{
    public class BoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
