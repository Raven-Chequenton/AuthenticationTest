using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuthenticationTest.Data;
using AuthenticationTest.Models;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class CompanyManagementController : Controller
{
    private readonly ApplicationDbContext _context;

    public CompanyManagementController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var companies = _context.Companies.ToList();
        return View(companies);
    }

    [HttpGet]
    public IActionResult CreateCompany()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany(Company model)
    {
        if (!ModelState.IsValid)
        {
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation error in {key}: {error.ErrorMessage}");
                }
            }
            return View(model);
        }

        _context.Companies.Add(model);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }


    [HttpPost]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company != null)
        {
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
