using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftSolutions.web.Application.Org;
using ShiftSolutions.web.Data;
using ShiftSolutions.web.Models;

public class BusinessRolesController : Controller
{
    private readonly AppDbContext _db;
    public BusinessRolesController(AppDbContext db) => _db = db;

    // LIST
    public async Task<IActionResult> Index()
        => View(await _db.BusinessRoles.OrderBy(x => x.Name).ToListAsync());

    // CREATE (you already added earlier)
    [HttpGet]
    public IActionResult Create() => View(new BusinessRoleCreateVm());

    [HttpPost]
    public async Task<IActionResult> Create(BusinessRoleCreateVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var exists = await _db.BusinessRoles.AnyAsync(x => x.Name == vm.Name);
        if (exists)
        {
            ModelState.AddModelError(nameof(vm.Name), "Role name already exists.");
            return View(vm);
        }

        _db.BusinessRoles.Add(new BusinessRole
        {
            Name = vm.Name.Trim(),
            Description = vm.Description,
            IsActive = vm.IsActive
        });
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // EDIT
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var role = await _db.BusinessRoles.FindAsync(id);
        if (role == null) return NotFound();

        var vm = new BusinessRoleCreateVm
        {
            Name = role.Name,
            Description = role.Description,
            IsActive = role.IsActive
        };
        ViewBag.RoleId = id;
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, BusinessRoleCreateVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var role = await _db.BusinessRoles.FindAsync(id);
        if (role == null) return NotFound();

        // enforce unique name (except self)
        var nameExists = await _db.BusinessRoles
            .AnyAsync(r => r.Id != id && r.Name == vm.Name);
        if (nameExists)
        {
            ModelState.AddModelError(nameof(vm.Name), "Role name already exists.");
            return View(vm);
        }

        role.Name = vm.Name.Trim();
        role.Description = vm.Description;
        role.IsActive = vm.IsActive;

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // DELETE
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var role = await _db.BusinessRoles.FindAsync(id);
        if (role == null) return NotFound();

        _db.BusinessRoles.Remove(role);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
