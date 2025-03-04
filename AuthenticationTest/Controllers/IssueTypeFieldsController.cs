using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationTest.Data;
using AuthenticationTest.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize(Roles = "Admin")] // ✅ Only Admins can create fields
public class IssueTypeFieldsController : Controller
{
    private readonly ApplicationDbContext _context;

    public IssueTypeFieldsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var fields = await _context.IssueTypeFields.Include(f => f.IssueType).ToListAsync();
        return View(fields);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.IssueTypes = new SelectList(_context.IssueTypes, "Id", "Name"); // ✅ Populate dropdown
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(IssueTypeField model)
    {
        if (ModelState.IsValid)
        {
            _context.IssueTypeFields.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        ViewBag.IssueTypes = new SelectList(_context.IssueTypes, "Id", "Name");
        return View(model);
    }
}

