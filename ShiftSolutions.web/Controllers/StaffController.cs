// Controllers/StaffController.cs
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;               // IWebHostEnvironment
using Microsoft.AspNetCore.Identity;             // UserManager
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShiftSolutions.web.Application.Org;        // StaffCreateWithUserVm
using ShiftSolutions.web.Data;                   // AppDbContext, ApplicationUser
using ShiftSolutions.web.Models;                 // Staff (your entity)

namespace ShiftSolutions.web.Controllers
{
    public class StaffController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _users;
        private readonly IWebHostEnvironment _env;

        public StaffController(
            AppDbContext db,
            UserManager<ApplicationUser> users,
            IWebHostEnvironment env)
        {
            _db = db;
            _users = users;
            _env = env;
        }

        // Controllers/StaffController.cs

        [HttpGet]
        public async Task<IActionResult> ListStaff(string? q)
        {
            var staffQ = _db.Staff.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var s = q.Trim().ToLower();
                staffQ = staffQ.Where(x =>
                    x.FirstName.ToLower().Contains(s) ||
                    x.LastName.ToLower().Contains(s) ||
                    (x.Email ?? "").ToLower().Contains(s) ||
                    (x.Phone ?? "").ToLower().Contains(s));
            }

            var items = await staffQ
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new StaffListItemVm
                {
                    Id = x.Id.ToString(),
                    FullName = x.FirstName + " " + x.LastName,
                    RoleName = _db.BusinessRoles.Where(r => r.Id == x.RoleId).Select(r => r.Name).FirstOrDefault() ?? "",
                    DepartmentName = _db.Departments.Where(d => d.Id == x.DepartmentId).Select(d => d.Name).FirstOrDefault() ?? "",
                    Email = x.Email ?? "",
                    Phone = x.Phone ?? "",
                    Status = x.Status,
                    AvatarUrl = string.IsNullOrWhiteSpace(x.AvatarUrl) ? "/images/users/avatar-3.jpg" : x.AvatarUrl!
                })
                .ToListAsync();

            var vm = new StaffListPageVm
            {
                Q = q,
                Items = items
            };

            return View(vm); // strongly-typed to StaffListPageVm
        }


        // ============ CREATE – GET ============
        [HttpGet]
        public async Task<IActionResult> NewStaff()
        {
            await LoadLookups();
            return View(new StaffCreateWithUserVm { DateJoined = DateTime.UtcNow.Date });
        }

        // ============ CREATE – POST ============
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewStaff(StaffCreateWithUserVm vm)
        {
            await LoadLookups();

            if (!ModelState.IsValid)
                return View(vm);

            // 1) Create Identity User
            var user = new ApplicationUser
            {
                UserName = vm.UserName,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber
            };

            var result = await _users.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);
                return View(vm);
            }

            // 2) Save avatar (optional)
            string? avatarUrl = null;
            if (vm.Avatar != null && vm.Avatar.Length > 0)
            {
                var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(vm.Avatar.FileName)}";
                var dir = Path.Combine(_env.WebRootPath, "uploads", "staff");
                Directory.CreateDirectory(dir);
                var full = Path.Combine(dir, fileName);
                using (var fs = System.IO.File.Create(full))
                    await vm.Avatar.CopyToAsync(fs);
                avatarUrl = $"/uploads/staff/{fileName}";
            }

            // 3) Create Staff row
            var staff = new Staff
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                Phone = vm.PhoneNumber,
                DepartmentId = vm.DepartmentId,
                RoleId = vm.BusinessRoleId,
                DateJoined = vm.DateJoined,
                AvatarUrl = avatarUrl,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id
            };

            _db.Staff.Add(staff);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(ListStaff));
        }

        // ============ PROFILE ============
        [HttpGet]
        public async Task<IActionResult> StaffProfile(int id)
        {
            var s = await _db.Staff.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (s == null) return NotFound();

            string? deptName = null, roleName = null;

            if (s.DepartmentId.HasValue)
                deptName = await _db.Departments
                    .Where(d => d.Id == s.DepartmentId.Value)
                    .Select(d => d.Name)
                    .FirstOrDefaultAsync();

            if (s.RoleId.HasValue)
                roleName = await _db.BusinessRoles
                    .Where(r => r.Id == s.RoleId.Value)
                    .Select(r => r.Name)
                    .FirstOrDefaultAsync();

            var vm = new
            {
                s.Id,
                s.FirstName,
                s.LastName,
                s.Email,
                s.Phone,
                s.Status,
                s.DateJoined,
                s.AvatarUrl,
                Department = deptName,
                Role = roleName
            };

            return View(vm);
        }

        // ============ helpers ============
        private async Task LoadLookups()
        {
            ViewBag.Departments = new SelectList(
                await _db.Departments.OrderBy(d => d.Name).ToListAsync(), "Id", "Name");

            ViewBag.BusinessRoles = new SelectList(
                await _db.BusinessRoles.OrderBy(r => r.Name).ToListAsync(), "Id", "Name");
        }
        // ============ EDIT – GET ============
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var s = await _db.Staff.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (s == null) return NotFound();

            await LoadLookups();

            var vm = new StaffEditVm
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email ?? "",
                Phone = s.Phone,
                DepartmentId = s.DepartmentId,
                BusinessRoleId = s.RoleId,
                Status = string.IsNullOrWhiteSpace(s.Status) ? "Active" : s.Status,
                DateJoined = s.DateJoined,
                ExistingAvatarUrl = s.AvatarUrl
            };

            return View(vm); // Views/Staff/Edit.cshtml
        }

        // ============ EDIT – POST ============
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StaffEditVm vm)
        {
            await LoadLookups();

            if (!ModelState.IsValid)
                return View(vm);

            var s = await _db.Staff.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (s == null) return NotFound();

            // Update Identity user email/phone (optional but nice)
            if (!string.IsNullOrWhiteSpace(s.UserId))
            {
                var user = await _users.FindByIdAsync(s.UserId);
                if (user != null)
                {
                    if (!string.Equals(user.Email, vm.Email, StringComparison.OrdinalIgnoreCase))
                        user.Email = vm.Email;
                    if (!string.Equals(user.PhoneNumber, vm.Phone, StringComparison.OrdinalIgnoreCase))
                        user.PhoneNumber = vm.Phone;
                    await _users.UpdateAsync(user);
                }
            }

            // Handle avatar upload (replace existing)
            if (vm.Avatar != null && vm.Avatar.Length > 0)
            {
                // delete old file (best-effort)
                if (!string.IsNullOrWhiteSpace(s.AvatarUrl) && s.AvatarUrl.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, s.AvatarUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(oldPath))
                    {
                        try { System.IO.File.Delete(oldPath); } catch { /* ignore */ }
                    }
                }

                var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(vm.Avatar.FileName)}";
                var dir = Path.Combine(_env.WebRootPath, "uploads", "staff");
                Directory.CreateDirectory(dir);
                var full = Path.Combine(dir, fileName);
                using (var fs = System.IO.File.Create(full))
                    await vm.Avatar.CopyToAsync(fs);

                s.AvatarUrl = $"/uploads/staff/{fileName}";
            }

            // Update fields
            s.FirstName = vm.FirstName;
            s.LastName = vm.LastName;
            s.Email = vm.Email;
            s.Phone = vm.Phone;
            s.DepartmentId = vm.DepartmentId;
            s.RoleId = vm.BusinessRoleId;
            s.Status = vm.Status;
            s.DateJoined = vm.DateJoined;
            s.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            // Back to list
            return RedirectToAction(nameof(ListStaff), new { q = Request.Query["q"].ToString() });
        }

        // ============ DELETE – POST ============
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var s = await _db.Staff.FirstOrDefaultAsync(x => x.Id == id);
            if (s == null) return NotFound();

            // Optional: cascade approach decisions
            // - If you want to delete the Identity user as well, uncomment below (be cautious):
            // if (!string.IsNullOrWhiteSpace(s.UserId))
            // {
            //     var user = await _users.FindByIdAsync(s.UserId);
            //     if (user != null) await _users.DeleteAsync(user);
            // }

            // Delete avatar file (best-effort)
            if (!string.IsNullOrWhiteSpace(s.AvatarUrl) && s.AvatarUrl.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase))
            {
                var oldPath = Path.Combine(_env.WebRootPath, s.AvatarUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(oldPath))
                {
                    try { System.IO.File.Delete(oldPath); } catch { /* ignore */ }
                }
            }

            _db.Staff.Remove(s);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(ListStaff));
        }

    }
}
