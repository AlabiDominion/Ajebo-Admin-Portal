using Microsoft.AspNetCore.Mvc;

namespace ShiftSolutions.web.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
