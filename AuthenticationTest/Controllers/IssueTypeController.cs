using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationTest.Data;
using AuthenticationTest.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin,Agent,Client")] // ✅ Allow Agents & Clients to view, restrict create, edit, delete to Admins
public class IssueTypeController : Controller
{
    private readonly ApplicationDbContext _context;

    public IssueTypeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ✅ Agents & Clients can view Issue Types
    public async Task<IActionResult> Index()
    {
        var issueTypes = await _context.IssueTypes.Include(it => it.IssueTypeFields).ToListAsync();
        return View(issueTypes);
    }

    // ✅ Admins Only: Create IssueType
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Create()
    {
        return View(new IssueTypeViewModel());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(IssueTypeViewModel model)
    {
        if (ModelState.IsValid)
        {
            var issueType = new IssueType { Name = model.Name };
            _context.IssueTypes.Add(issueType);
            await _context.SaveChangesAsync(); // ✅ Save first to generate IssueTypeId

            if (model.Fields != null && model.Fields.Any())
            {
                foreach (var field in model.Fields)
                {
                    var newField = new IssueTypeField
                    {
                        FieldName = field.FieldName,
                        FieldType = field.FieldType,
                        IssueTypeId = issueType.Id // ✅ Assign IssueTypeId AFTER saving IssueType
                    };
                    _context.IssueTypeFields.Add(newField);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        return View(model);
    }

    // ✅ Admins Only: Edit IssueType
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var issueType = await _context.IssueTypes
            .Include(it => it.IssueTypeFields)
            .FirstOrDefaultAsync(it => it.Id == id);

        if (issueType == null)
        {
            return NotFound();
        }

        var model = new EditIssueTypeViewModel
        {
            Id = issueType.Id,
            Name = issueType.Name,
            Fields = issueType.IssueTypeFields
                .Select(f => new IssueTypeFieldViewModel
                {
                    Id = f.Id,
                    FieldName = f.FieldName,
                    FieldType = f.FieldType
                }).ToList()
        };

        return View(model); // ✅ Ensure it returns EditIssueTypeViewModel
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Edit(EditIssueTypeViewModel model)
    {
        var issueType = await _context.IssueTypes
            .Include(it => it.IssueTypeFields)
            .FirstOrDefaultAsync(it => it.Id == model.Id);

        if (issueType == null)
        {
            return NotFound();
        }

        issueType.Name = model.Name;

        // ✅ Log submitted fields
        Console.WriteLine($"🔍 DEBUG: Submitting {model.Fields.Count} fields for IssueType ID {model.Id}");

        // ✅ Remove fields that were deleted in the UI
        var existingFields = issueType.IssueTypeFields.ToList();
        foreach (var existingField in existingFields)
        {
            if (!model.Fields.Any(f => f.Id == existingField.Id))
            {
                Console.WriteLine($"🗑️ Removing field {existingField.Id}");
                _context.IssueTypeFields.Remove(existingField);
            }
        }

        // ✅ Add or update fields
        foreach (var field in model.Fields)
        {
            if (field.Id > 0) // ✅ Update existing field
            {
                var existingField = issueType.IssueTypeFields.FirstOrDefault(f => f.Id == field.Id);
                if (existingField != null)
                {
                    Console.WriteLine($"✏️ Updating field {existingField.Id}");
                    existingField.FieldName = field.FieldName;
                    existingField.FieldType = field.FieldType;
                }
            }
            else // ✅ Add new field
            {
                var newField = new IssueTypeField
                {
                    FieldName = field.FieldName,
                    FieldType = field.FieldType,
                    IssueTypeId = model.Id // ✅ Ensure IssueTypeId is assigned
                };
                Console.WriteLine($"➕ Adding new field: {newField.FieldName}");
                _context.IssueTypeFields.Add(newField);
            }
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    // ✅ Admins Only: Delete IssueType
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var issueType = await _context.IssueTypes
            .Include(it => it.IssueTypeFields) // ✅ Ensure fields are loaded before deleting
            .FirstOrDefaultAsync(it => it.Id == id);

        if (issueType != null)
        {
            _context.IssueTypeFields.RemoveRange(issueType.IssueTypeFields); // ✅ Delete related fields
            _context.IssueTypes.Remove(issueType);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
