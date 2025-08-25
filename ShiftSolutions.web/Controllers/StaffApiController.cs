// Controllers/StaffApiController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftSolutions.web.Data;

[Authorize]
[ApiController]
[Route("api/staff")]
public class StaffApiController : ControllerBase
{
    private readonly AppDbContext _db;
    public StaffApiController(AppDbContext db) => _db = db;

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
}
