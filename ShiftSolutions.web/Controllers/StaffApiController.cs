// Controllers/StaffApiController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftSolutions.web.Application.Merchants;
using ShiftSolutions.web.Data;
using ShiftSolutions.web.Services;

namespace ShiftSolutions.web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/staff")]
    public class StaffApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMerchantService _merchants;

        public StaffApiController(AppDbContext db, IMerchantService merchants)
        {
            _db = db;
            _merchants = merchants;
        }

        [HttpGet("options")]
        public async Task<IActionResult> Options(CancellationToken ct)
        {
            var options = await _db.Staff
                .Where(s => s.Status == "Active")
                .OrderBy(s => s.FirstName).ThenBy(s => s.LastName)
                .Select(s => new { id = s.Id, name = s.FirstName + " " + s.LastName + " • " + s.Email })
                .ToListAsync(ct);

            return Ok(options);
        }

        [HttpGet("{staffId:int}/merchants")]
        public async Task<IActionResult> GetMerchantsForStaff(
            int staffId,
            [FromQuery] MerchantFilter filter,
            CancellationToken ct)
        {
            filter.Sort ??= "created_desc";
            var result = await _merchants.GetMerchantsForStaffAsync(staffId, filter, ct);
            return Ok(result);
        }
    }
}
