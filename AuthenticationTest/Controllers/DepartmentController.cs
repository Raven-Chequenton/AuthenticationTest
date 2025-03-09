using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AuthenticationTest.Data;
using AuthenticationTest.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public class DepartmentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager; // ✅ Inject UserManager to fetch roles

    public DepartmentController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager; // ✅ Initialize UserManager
    }

    // ✅ GET: Edit Department
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var department = await _context.Departments
            .Include(d => d.UserDepartments)
            .ThenInclude(ud => ud.User)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department == null)
        {
            return NotFound();
        }

        var users = await _userManager.Users.ToListAsync();

        var model = new EditDepartmentModel
        {
            Id = department.Id,
            Name = department.Name,
            AssignedUserIds = department.UserDepartments.Select(ud => ud.UserId).ToList(),
            AllUsers = users.Select(u => new UserViewModel { Id = u.Id, Username = u.UserName }).ToList()
        };

        return View(model);
    }

    // ✅ POST: Edit Department
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditDepartmentModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var department = await _context.Departments
            .Include(d => d.UserDepartments)
            .FirstOrDefaultAsync(d => d.Id == model.Id);

        if (department == null)
        {
            return NotFound();
        }

        department.Name = model.Name;

        // Update assigned users
        _context.UserDepartments.RemoveRange(department.UserDepartments);

        if (model.AssignedUserIds != null)
        {
            foreach (var userId in model.AssignedUserIds)
            {
                _context.UserDepartments.Add(new UserDepartment
                {
                    DepartmentId = department.Id,
                    UserId = userId
                });
            }
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Index()
    {
        var departments = await _context.Departments
            .Select(d => new DepartmentViewModel
            {
                Id = d.Id,
                Name = d.Name,
                AssignedUsers = _context.UserDepartments
                    .Where(ud => ud.DepartmentId == d.Id)
                    .Select(ud => new UserViewModel
                    {
                        Id = ud.User.Id,
                        Username = ud.User.UserName
                    })
                    .ToList()
            })
            .ToListAsync();

        // ✅ Fetch roles for each user
        foreach (var department in departments)
        {
            foreach (var user in department.AssignedUsers)
            {
                var identityUser = await _userManager.FindByIdAsync(user.Id);
                var roles = await _userManager.GetRolesAsync(identityUser);
                user.Role = roles.Any() ? string.Join(", ", roles) : "No Role"; // ✅ Assign role(s)
            }
        }

        return View(departments);
    }
}
