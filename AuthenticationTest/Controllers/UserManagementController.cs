using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AuthenticationTest.Data;
using AuthenticationTest.Models;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class UserManagementController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;

    public UserManagementController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        var roles = _roleManager.Roles.ToList();
        var companies = _context.Companies.ToList();

        // ✅ Dictionary to store user roles
        var userRoles = new Dictionary<string, string>();
        foreach (var user in users)
        {
            var rolesForUser = await _userManager.GetRolesAsync(user);
            userRoles[user.Id] = rolesForUser.FirstOrDefault() ?? "No Role";
        }

        // ✅ Dictionary to store user-company relationships
        var userCompanies = new Dictionary<string, string>();
        var userCompanyRecords = _context.UserCompanies.ToList();
        foreach (var userCompany in userCompanyRecords)
        {
            var company = companies.FirstOrDefault(c => c.Id == userCompany.CompanyId);
            if (company != null)
            {
                userCompanies[userCompany.UserId] = company.Name;
            }
        }

        var viewModel = new UserManagementViewModel
        {
            Users = users,
            Companies = companies,
            UserRoles = userRoles,
            UserCompanies = userCompanies
        };

        return View(viewModel);
    }


    [HttpGet]
    public IActionResult CreateUser()
    {
        ViewBag.Roles = _roleManager.Roles.ToList(); // ✅ Send available roles to the view
        ViewBag.Companies = _context.Companies.ToList(); // ✅ Send companies for selection
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(RegisterUserModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // ✅ Ensure role exists before assigning
                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Role));
                }

                // ✅ Assign Role to the User
                await _userManager.AddToRoleAsync(user, model.Role);

                // ✅ Assign User to a Company if selected
                if (model.CompanyId.HasValue)
                {
                    var company = await _context.Companies.FindAsync(model.CompanyId);
                    if (company != null)
                    {
                        _context.UserCompanies.Add(new UserCompany
                        {
                            UserId = user.Id,
                            CompanyId = company.Id
                        });
                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        ViewBag.Roles = _roleManager.Roles.ToList();
        ViewBag.Companies = _context.Companies.ToList();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var roles = await _userManager.GetRolesAsync(user);
        var companies = _context.Companies.ToList();

        var model = new EditUserModel
        {
            Id = user.Id,
            Email = user.Email,
            SelectedRole = roles.FirstOrDefault(),
            CompanyId = companies.FirstOrDefault()?.Id,
            AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList(),
            AvailableCompanies = companies
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditUserModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null)
        {
            return NotFound();
        }

        // ✅ Update Email & Username
        user.Email = model.Email;
        user.UserName = model.Email;
        await _userManager.UpdateAsync(user);

        // ✅ Update Role
        var existingRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, existingRoles);
        await _userManager.AddToRoleAsync(user, model.SelectedRole);

        // ✅ Update Company Assignment
        var existingCompany = _context.UserCompanies.FirstOrDefault(uc => uc.UserId == user.Id);
        if (existingCompany != null)
        {
            _context.UserCompanies.Remove(existingCompany); // Remove old company
        }

        if (model.CompanyId.HasValue)
        {
            _context.UserCompanies.Add(new UserCompany
            {
                UserId = user.Id,
                CompanyId = model.CompanyId.Value
            });
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    [HttpPost]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            // ✅ Remove User-Company Relationship
            var userCompany = _context.UserCompanies.FirstOrDefault(uc => uc.UserId == id);
            if (userCompany != null)
            {
                _context.UserCompanies.Remove(userCompany);
                await _context.SaveChangesAsync();
            }

            await _userManager.DeleteAsync(user);
        }
        return RedirectToAction("Index");
    }
}
