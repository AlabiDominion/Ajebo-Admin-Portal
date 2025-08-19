using Microsoft.AspNetCore.Mvc;
using ShiftSolutions.web.Data;
using System.Data.Entity;

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
            var apartments =await _appDbContext.Apartments.ToListAsync();
            ViewBag.Apartments = apartments;
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
