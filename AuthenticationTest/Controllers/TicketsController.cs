using AuthenticationTest.Data;
using AuthenticationTest.Models;
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
            var tickets = await _context.Tickets
                .Include(t => t.Company)
                .Include(t => t.Circuit)
                .Include(t => t.IssueType)
                .OrderByDescending(t => t.CreatedOn)
                .ToListAsync();

            return View(tickets);
        }

        // ✅ Create Ticket Page
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            // ✅ Fetch user Company
            var userCompany = await _context.UserCompanies
                .Include(uc => uc.Company)
                .FirstOrDefaultAsync(uc => uc.UserId == user.Id);

            ViewBag.CompanyName = userCompany?.Company.Name;
            ViewBag.CompanyId = userCompany?.CompanyId;
            ViewBag.Companies = roles.Contains("Client") ? null : new SelectList(await _context.Companies.ToListAsync(), "Id", "Name");

            // ✅ Circuit IDs Filtered for Client & Admin/Agent
            ViewBag.CircuitIds = new SelectList(
                await _context.Circuits
                    .Where(c => roles.Contains("Client") ? c.CompanyId == userCompany.CompanyId : true)
                    .Select(c => new { c.Id, Display = $"{c.CircuitID} - {c.SiteName}" })
                    .ToListAsync(),
                "Id", "Display");

            // ✅ Populate Issue Types
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

        // ✅ Create Ticket POST
        [HttpPost]
        public async Task<IActionResult> Create(Ticket ticket, List<IFormFile> attachments)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var issueType = await _context.IssueTypes.FindAsync(ticket.IssueTypeId);
            var circuit = await _context.Circuits.FindAsync(ticket.CircuitId);

            if (issueType == null || circuit == null)
            {
                ModelState.AddModelError("", "Issue Type or Circuit ID is invalid.");
                return View(ticket);
            }

            // ✅ Set Requestor as Username
            ticket.RequestorUsername = user.Email;
            ticket.CreatedOn = DateTime.UtcNow;
            ticket.Status = "Unassigned";
            ticket.TicketRef = $"ARC-{(_context.Tickets.Count() + 1).ToString("D6")}";
            ticket.ShortDescription = $"{issueType.Name} - {circuit.CircuitID}";

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

            ViewBag.Companies = new SelectList(_context.Companies, "Id", "Name", ticket.CompanyId);
            ViewBag.CircuitIds = new SelectList(_context.Circuits, "CircuitID", "CircuitID", ticket.CircuitId);
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
