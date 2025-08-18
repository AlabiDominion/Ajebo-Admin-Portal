using Microsoft.AspNetCore.Mvc;

namespace ShiftSolutions.web.Controllers
{
    public class MerchantController : Controller
    {
        public IActionResult MerchantList()
        {
            return View();
        }
        public IActionResult MerchantProfile()
        {
            return View();
        }
    }
}
