using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationTest.Data;
using AuthenticationTest.Models;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]  // ✅ Restrict all actions to Admins only
public class DepartmentController : Controller
{
    private readonly ApplicationDbContext _context;

    public DepartmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var departments = _context.Departments.ToList();
        return View(departments);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Department model)
    {
        if (ModelState.IsValid)
        {
            _context.Departments.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(model);
    }



    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department != null)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
