using Microsoft.AspNetCore.Mvc;
using ShiftSolutions.web.Data;

namespace ShiftSolutions.web.Controllers
{
    public class OpTransactionController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public OpTransactionController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> OrdersBooking()
        {
            return View();
        }
        public IActionResult OrdersDetailes()
        {
            return View();
        }
        public IActionResult Settlements()
        {
            return View();
        }
        public IActionResult BatchDetailes()
        {
            return View();
        }
    }

    public class Async
    {
    }
}
