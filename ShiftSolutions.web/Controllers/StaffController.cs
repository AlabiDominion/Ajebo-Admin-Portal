using Microsoft.AspNetCore.Mvc;

namespace ShiftSolutions.web.Controllers
{
    public class StaffController : Controller
    {
        public IActionResult ListStaff()
        {
            return View();
        }
        public IActionResult NewStaff()
        {
            return View();
        }
    }
}
