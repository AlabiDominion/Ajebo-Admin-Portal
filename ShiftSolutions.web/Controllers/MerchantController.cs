using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShiftSolutions.web.Application.Merchants;  // MerchantFilter, DTOs, PagedResult
using ShiftSolutions.web.Data;                   // ApplicationUser
using ShiftSolutions.web.Services;               // IMerchantService

namespace ShiftSolutions.web.Controllers
{
    [Authorize]
    public class MerchantController : Controller
    {
        private readonly IMerchantService _svc;
        private readonly UserManager<ApplicationUser> _users;

        public MerchantController(IMerchantService svc, UserManager<ApplicationUser> users)
        {
            _svc = svc;
            _users = users;
        }

        // ==================== LIST PAGE ====================

        // Renders the list page 
        [HttpGet]
        public IActionResult MerchantList() => View();

        // Data endpoint the list page calls via fetch/Ajax
        // Example query: /Merchant/ListData?page=1&pageSize=20&status=Approved&search=chi
        [HttpGet]
        public async Task<IActionResult> ListData([FromQuery] MerchantFilter filter, CancellationToken ct)
        {
            var result = await _svc.GetMerchantsAsync(filter, ct);
            return Json(result); // { items, page, pageSize, totalItems }
        }

        // ==================== PROFILE ======================

        [HttpGet]
        public async Task<IActionResult> Profile(string id, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            var dto = await _svc.GetMerchantAsync(id, ct);
            if (dto == null) return NotFound();

            return View("MerchantProfile", dto); 
        }

        // ==================== APPROVE / DECLINE ============

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Approve([FromForm] string agentId, int assignedStaffId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(agentId)) return BadRequest("Missing merchant id.");

            var approvedByUserId = _users.GetUserId(User) ?? "system";
            await _svc.ApproveMerchantAsync(agentId, approvedByUserId, assignedStaffId, ct);

            return Ok(new { ok = true, message = "Merchant approved." });
        }

        public sealed class DeclineInput
        {
            public string Id { get; set; } = default!;
            public string Reason { get; set; } = default!;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Decline([FromForm] DeclineInput input, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(input?.Id) || string.IsNullOrWhiteSpace(input.Reason))
                return BadRequest("Merchant id and reason are required.");

            var userId = _users.GetUserId(User) ?? "system";
            await _svc.DeclineMerchantAsync(input.Id, input.Reason, userId, ct);

            return Ok(new { ok = true, message = "Merchant declined." });
        }
    }
}
