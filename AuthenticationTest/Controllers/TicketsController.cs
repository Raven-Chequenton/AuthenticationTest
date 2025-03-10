using AuthenticationTest.Data;
using AuthenticationTest.Models;
using AuthenticationTest.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationTest.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TicketsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ Index Page
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            var userCompany = await _context.UserCompanies
                .Include(uc => uc.Company)
                .FirstOrDefaultAsync(uc => uc.UserId == user.Id);

            ViewBag.CompanyName = userCompany?.Company.Name;
            ViewBag.CompanyId = userCompany?.CompanyId;

            var tickets = await _context.Tickets
                .Include(t => t.IssueType)
                .Include(t => t.Circuit)
                .Include(t => t.Company)
                .AsNoTracking()
                .ToListAsync(); // ✅ Fetch from database first

            // ✅ Convert to ViewModel AFTER fetching
            var ticketViewModels = tickets
                .Select(t => new TicketViewModel
                {
                    Id = t.Id,
                    TicketRef = t.TicketRef,
                    RequestorEmail = t.RequestorEmail ?? "N/A",
                    ShortDescription = $"{(t.IssueType != null ? t.IssueType.Name : "N/A")} - {(t.Circuit != null ? t.Circuit.CircuitID : "N/A")}",
                    CompanyName = t.Company?.Name ?? "N/A",
                    DepartmentId = t.DepartmentId,
                    Status = t.Status ?? "Unknown",
                    CreatedOn = t.CreatedOn,
                    UpdatedOn = t.UpdatedOn ?? DateTime.UtcNow
                })
                .ToList();

            return View(ticketViewModels);
        }


        // ✅ Create Ticket Page
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            var userCompany = await _context.UserCompanies
                .Include(uc => uc.Company)
                .FirstOrDefaultAsync(uc => uc.UserId == user.Id);

            ViewBag.CompanyName = userCompany?.Company.Name;
            ViewBag.CompanyId = userCompany?.CompanyId;
            ViewBag.Companies = roles.Contains("Client") ? null : new SelectList(await _context.Companies.ToListAsync(), "Id", "Name");

            // ✅ Fetch Circuits First, Then Apply String Interpolation
            var circuits = await _context.Circuits
                .Where(c => roles.Contains("Client") ? c.CompanyId == userCompany.CompanyId : true)
                .ToListAsync(); // ✅ Fetch from DB first

            ViewBag.CircuitIds = new SelectList(
                circuits.Select(c => new { c.Id, Display = $"{c.CircuitID} - {c.SiteName}" }), // ✅ Apply String Interpolation in Memory
                "Id", "Display");

            ViewBag.IssueTypes = new SelectList(await _context.IssueTypes.ToListAsync(), "Id", "Name");

            return View();
        }


        // ✅ Fetch Circuit Details (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetCircuitDetails(int id)
        {
            var circuit = await _context.Circuits.FindAsync(id);
            if (circuit == null) return NotFound();
            return Json(new { siteName = circuit.SiteName, vlan = circuit.VLAN });
        }

        // ✅ Fetch Issue Type Fields (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetIssueFields(int id)
        {
            var fields = await _context.IssueTypeFields
                .Where(f => f.IssueTypeId == id)
                .ToListAsync();

            return PartialView("_IssueTypeFields", fields);
        }

        [HttpPost]
        public async Task<IActionResult> AddInternalNote(int ticketId, string note)
        {
            if (ticketId <= 0 || string.IsNullOrWhiteSpace(note))
            {
                return BadRequest("Invalid ticket ID or empty note.");
            }

            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
            if (ticket == null)
            {
                return NotFound("Ticket not found.");
            }

            // Append the new note to the existing history
            ticket.InternalNotesHistory = (ticket.InternalNotesHistory ?? "") + $"\n{User.Identity.Name} - {note} - {DateTime.UtcNow}";

            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Note added successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> SendCustomerMessage(int ticketId, string message)
        {
            if (ticketId <= 0 || string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Invalid ticket ID or empty message.");
            }

            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
            if (ticket == null)
            {
                return NotFound("Ticket not found.");
            }

            // Append the new message to the existing Customer Communication History
            ticket.CustomerCommunicationHistory = (ticket.CustomerCommunicationHistory ?? "") +
                $"\n{User.Identity.Name} - {message} - {DateTime.UtcNow}";

            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Customer message added successfully." });
        }



        // ✅ Create Ticket POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket, List<IFormFile> attachments, Dictionary<string, string> dynamicFields)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // ✅ Generate TicketRef
            ticket.TicketRef = $"ARC-{(_context.Tickets.Count() + 1).ToString("D6")}";

            // ✅ Fetch Circuit Details
            var circuit = await _context.Circuits.FindAsync(ticket.CircuitId);
            if (circuit != null)
            {
                ticket.VLAN = circuit.VLAN;
                ticket.SiteName = circuit.SiteName;
            }
            else
            {
                ticket.VLAN = "N/A";
                ticket.SiteName = "N/A";
            }

            // ✅ Ensure Required Fields Have Default Values
            ticket.Status = "Unassigned";
            ticket.CreatedOn = DateTime.UtcNow;
            ticket.RequestorEmail = user.Email;

            // ✅ Build Internal Notes with Dynamic Fields
            string internalNotesLog = $@"
    <strong>Ticket Created:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} <br/>
    <strong>Requestor:</strong> {user.Email} <br/>
    <strong>Company:</strong> {(ticket.CompanyId != 0 ? _context.Companies.Find(ticket.CompanyId)?.Name : "N/A")} <br/>
    <strong>Circuit ID:</strong> {(circuit?.CircuitID ?? "N/A")} <br/>
    <strong>Issue Type:</strong> {(ticket.IssueTypeId != 0 ? _context.IssueTypes.Find(ticket.IssueTypeId)?.Name : "N/A")} <br/>
    ";

            // ✅ Append Dynamic Fields
            if (dynamicFields != null && dynamicFields.Count > 0)
            {
                internalNotesLog += "<strong>Dynamic Fields:</strong> <br/>";
                foreach (var field in dynamicFields)
                {
                    internalNotesLog += $"{field.Key}: {field.Value} <br/>";
                }
            }

            ticket.InternalNotesHistory = internalNotesLog;

            // ✅ Validate model
            if (!ModelState.IsValid)
            {
                return View(ticket);
            }

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            // ✅ Handle Attachments
            if (attachments != null)
            {
                foreach (var file in attachments)
                {
                    var filePath = Path.Combine("wwwroot/uploads", file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    _context.TicketAttachments.Add(new TicketAttachment
                    {
                        TicketId = ticket.Id,
                        FileName = file.FileName,
                        FilePath = "/uploads/" + file.FileName,
                        UploadedBy = user.UserName,
                        UploadedOn = DateTime.UtcNow
                    });
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // ✅ Edit Ticket
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            { 
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.TicketAttachments)
                .Include(t => t.TicketFields)
                .Include(t => t.Assignee)
                .Include(t => t.Company)
                .Include(t => t.Circuit)
                .Include(t => t.IssueType)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            // ✅ Ensure navigation properties are initialized correctly
            ticket.Company ??= new Company { Name = "N/A" };

            

            ticket.IssueType ??= new IssueType { Name = "N/A" };
            ticket.Assignee ??= new IdentityUser { UserName = "Unassigned" };
            ticket.TicketAttachments ??= new List<TicketAttachment>();
            ticket.TicketFields ??= new List<TicketField>();

            // ✅ Handle other nullable values safely
            ticket.Status = string.IsNullOrEmpty(ticket.Status) ? "Open" : ticket.Status;
            ticket.ProviderRef = string.IsNullOrEmpty(ticket.ProviderRef) ? "N/A" : ticket.ProviderRef;
            ticket.CC = string.IsNullOrEmpty(ticket.CC) ? "N/A" : ticket.CC;
            ticket.InternalNotesHistory = string.IsNullOrEmpty(ticket.InternalNotesHistory) ? "No internal notes yet." : ticket.InternalNotesHistory;
            ticket.CustomerCommunicationHistory = string.IsNullOrEmpty(ticket.CustomerCommunicationHistory) ? "No customer communication yet." : ticket.CustomerCommunicationHistory;
            ticket.CreatedOn = ticket.CreatedOn == DateTime.MinValue ? DateTime.Now : ticket.CreatedOn;
            ticket.UpdatedOn = DateTime.Now;
            ticket.RequestorEmail = string.IsNullOrEmpty(ticket.RequestorEmail) ? "No requestor email" : ticket.RequestorEmail;

            // ✅ Populate ViewBag for dropdown selections
            ViewBag.Companies = new SelectList(_context.Companies, "Id", "Name", ticket.CompanyId);
            ViewBag.CircuitIds = new SelectList(_context.Circuits, "Id", "CircuitID", ticket.CircuitId ?? 0);
            ViewBag.IssueTypes = new SelectList(_context.IssueTypes, "Id", "Name", ticket.IssueTypeId);
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name");

            return View(ticket);
        }


        // ✅ Get Assignees by Department (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetAssigneesByDepartment(int departmentId)
        {
            var assignees = await _context.Users
                .Where(u => _context.UserDepartments
                    .Any(ud => ud.DepartmentId == departmentId && ud.UserId == u.Id))
                .Select(u => new { Id = u.Id, Email = u.Email })
                .ToListAsync();

            return Json(assignees);
        }

        // ✅ Save Changes
        [HttpPost]
        public async Task<IActionResult> SaveChanges(int ticketId, string providerRef, int? departmentId, string? assigneeId, string? cc)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return NotFound();

            ticket.ProviderRef = providerRef;
            ticket.DepartmentId = departmentId;
            ticket.AssigneeId = assigneeId;
            ticket.CC = cc;
            ticket.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok();
        }

        // ✅ Update Ticket Status
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int ticketId, string status)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return NotFound();

            ticket.Status = status;
            ticket.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
