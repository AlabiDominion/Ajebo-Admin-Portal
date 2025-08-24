using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftSolutions.web.Application.Org;
using ShiftSolutions.web.Data;
using ShiftSolutions.web.Models;

public class DepartmentsController : Controller
{
    private readonly AppDbContext _db;
    public DepartmentsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
        => View(await _db.Departments.OrderBy(x => x.Name).ToListAsync());

    [HttpGet]
    public IActionResult Create() => View(new DepartmentCreateVm());

    [HttpPost]
    public async Task<IActionResult> Create(DepartmentCreateVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var exists = await _db.Departments.AnyAsync(x => x.Name == vm.Name);
        if (exists)
        {
            ModelState.AddModelError(nameof(vm.Name), "Department name already exists.");
            return View(vm);
        }

        _db.Departments.Add(new Department
        {
            Name = vm.Name.Trim(),
            Description = vm.Description,
            IsActive = vm.IsActive
        });
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    // GET: /Departments/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var dept = await _db.Departments.FindAsync(id);
        if (dept == null) return NotFound();

        var vm = new DepartmentCreateVm
        {
            Name = dept.Name,
            Description = dept.Description,
            IsActive = dept.IsActive
        };
        ViewBag.DepartmentId = id;
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, DepartmentCreateVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var dept = await _db.Departments.FindAsync(id);
        if (dept == null) return NotFound();

        dept.Name = vm.Name.Trim();
        dept.Description = vm.Description;
        dept.IsActive = vm.IsActive;
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // POST: /Departments/Delete/5
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var dept = await _db.Departments.FindAsync(id);
        if (dept == null) return NotFound();

        _db.Departments.Remove(dept);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

}
