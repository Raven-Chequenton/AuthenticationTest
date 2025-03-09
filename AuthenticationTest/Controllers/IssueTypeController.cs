using AuthenticationTest.Data;
using AuthenticationTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class IssueTypeController : Controller
{
    private readonly ApplicationDbContext _context;

    public IssueTypeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ✅ List all Issue Types
    public async Task<IActionResult> Index()
    {
        var issueTypes = await _context.IssueTypes
            .Include(it => it.IssueTypeFields)
            .ToListAsync();
        return View(issueTypes);
    }

    // ✅ Create new Issue Type (GET)
    public IActionResult Create()
    {
        return View();
    }

    // ✅ Create new Issue Type (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IssueType model, List<string> fieldNames, List<string> fieldTypes)
    {
        if (ModelState.IsValid)
        {
            _context.IssueTypes.Add(model);
            await _context.SaveChangesAsync();

            // Add Fields
            for (int i = 0; i < fieldNames.Count; i++)
            {
                if (!string.IsNullOrEmpty(fieldNames[i]))
                {
                    _context.IssueTypeFields.Add(new IssueTypeField
                    {
                        IssueTypeId = model.Id,
                        FieldName = fieldNames[i],
                        FieldType = fieldTypes[i]
                    });
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // ✅ Edit Issue Type (GET)
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var issueType = await _context.IssueTypes
            .Include(i => i.IssueTypeFields)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (issueType == null)
        {
            return NotFound();
        }

        var viewModel = new IssueTypeViewModel
        {
            Id = issueType.Id,
            Name = issueType.Name,
            Fields = issueType.IssueTypeFields.Select(f => new IssueTypeFieldViewModel
            {
                Id = f.Id,
                FieldName = f.FieldName,
                FieldType = f.FieldType
            }).ToList()
        };

        return View(viewModel);
    }

    // ✅ Edit Issue Type (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(IssueTypeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var issueType = await _context.IssueTypes
            .Include(i => i.IssueTypeFields)
            .FirstOrDefaultAsync(i => i.Id == model.Id);

        if (issueType == null)
        {
            return NotFound();
        }

        issueType.Name = model.Name;

        // ✅ Remove fields that were deleted in the UI
        var existingFieldIds = model.Fields.Select(f => f.Id).ToList();
        issueType.IssueTypeFields = issueType.IssueTypeFields
            .Where(f => existingFieldIds.Contains(f.Id))
            .ToList();

        // ✅ Add or update fields
        foreach (var field in model.Fields)
        {
            var existingField = issueType.IssueTypeFields.FirstOrDefault(f => f.Id == field.Id);
            if (existingField != null)
            {
                existingField.FieldName = field.FieldName;
                existingField.FieldType = field.FieldType;
            }
            else
            {
                issueType.IssueTypeFields.Add(new IssueTypeField
                {
                    FieldName = field.FieldName,
                    FieldType = field.FieldType,
                    IssueTypeId = issueType.Id // Ensure it's linked correctly
                });
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    // ✅ Delete Issue Type
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var issueType = await _context.IssueTypes.FindAsync(id);
        if (issueType == null) return NotFound();

        _context.IssueTypes.Remove(issueType);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
