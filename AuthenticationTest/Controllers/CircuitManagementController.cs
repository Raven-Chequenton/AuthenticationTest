using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuthenticationTest.Data;
using AuthenticationTest.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

[Authorize(Roles = "Admin,Agent")] // ✅ Admins & Agents can access
public class CircuitManagementController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<CircuitManagementController> _logger;

    public CircuitManagementController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager, // ✅ Inject UserManager
        ILogger<CircuitManagementController> logger)
    {
        _context = context;
        _userManager = userManager; // ✅ Assign UserManager
        _logger = logger;
    }

    // ✅ Agents & Admins Can View Circuits
    public IActionResult Index()
    {
        var circuits = _context.Circuits
            .Include(c => c.Company) // Ensure Company data is loaded
            .ToList(); // ✅ Now it returns a List<Circuit>

        return View(circuits);
    }


    // ✅ Admins Only: Create Circuit (GET)
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Companies = _context.Companies.ToList(); // ✅ Populate company dropdown
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Circuit model)
    {
        _logger.LogInformation("🔄 Incoming Circuit Data: CircuitID={CircuitID}, Site={SiteName}, VLAN={VLAN}, CompanyId={CompanyId}",
            model.CircuitID, model.SiteName, model.VLAN, model.CompanyId);

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogWarning("⚠️ Could not find logged-in user.");
            ModelState.AddModelError("", "Could not identify the user.");
        }
        else
        {
            model.CreatedBy = user.UserName; // ✅ Assign logged-in user
        }

        // ✅ Ensure CreatedOn is set
        model.CreatedOn = DateTime.UtcNow;

        var company = await _context.Companies.FindAsync(model.CompanyId);
        if (company == null)
        {
            _logger.LogWarning("⚠️ Selected CompanyId={CompanyId} does not exist in database!", model.CompanyId);
            ModelState.AddModelError("Company", "The selected company does not exist.");
        }
        else
        {
            model.Company = company; // ✅ Assign navigation property
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Circuits.Add(model);
                await _context.SaveChangesAsync();

                _logger.LogInformation("✅ Circuit Created Successfully: {CircuitID}, CreatedOn={CreatedOn}", model.CircuitID, model.CreatedOn);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error while creating Circuit: {CircuitID}", model.CircuitID);
                ModelState.AddModelError("", "An error occurred while saving the circuit.");
            }
        }
        else
        {
            _logger.LogWarning("⚠️ Circuit creation failed due to invalid model state.");
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    _logger.LogWarning("❗ ModelState Error: {Field} - {Message}", state.Key, error.ErrorMessage);
                }
            }
        }

        ViewBag.Companies = _context.Companies.ToList();
        return View(model);
    }





    // ✅ Admins Only: Edit Circuit
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var circuit = await _context.Circuits.FindAsync(id);
        if (circuit == null) return NotFound();

        ViewBag.Companies = _context.Companies.ToList(); // ✅ Populate company dropdown
        return View(circuit);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Edit(Circuit model)
    {
        if (ModelState.IsValid)
        {
            _context.Circuits.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        ViewBag.Companies = _context.Companies.ToList();
        return View(model);
    }

    // ✅ Admins Only: Delete Circuit
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var circuit = await _context.Circuits.FindAsync(id);
        if (circuit != null)
        {
            _context.Circuits.Remove(circuit);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
