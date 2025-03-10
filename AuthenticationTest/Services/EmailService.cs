using System;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using MailKit;
using MimeKit;
using AuthenticationTest.Data;
using AuthenticationTest.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;
using AuthenticationTest.Services;
using IEmailService = AuthenticationTest.Services.IEmailService;

public class EmailService : IEmailService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EmailService> _logger;

    public EmailService(ApplicationDbContext context, ILogger<EmailService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ProcessEmailsAsync()
    {
        try
        {
            using (var client = new ImapClient())
            {
                client.Connect("smtp.office365.com", 993, SecureSocketOptions.SslOnConnect);
                client.Authenticate("aihelpdesk@arcinternational.co.za", "ScZHG5s1jocoNP3X812V86");

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadWrite);

                foreach (var message in inbox.Fetch(0, -1, MessageSummaryItems.UniqueId))
                {
                    var email = inbox.GetMessage(message.UniqueId);

                    // Check if ticket already exists
                    if (_context.Tickets.Any(t => t.ShortDescription == email.Subject)) continue;

                    var requestorEmail = email.From.Mailboxes.FirstOrDefault()?.Address;
                    var requestor = await _context.Users.FirstOrDefaultAsync(u => u.Email == requestorEmail);

                    // Create a new ticket
                    var newTicket = new Ticket
                    {
                        RequestorEmail = requestorEmail,
                        IssueTypeId = GetIssueTypeIdFromSubject(email.Subject),
                        ShortDescription = email.Subject,
                        Status = "Unassigned",
                        CustomerCommunicationHistory = $"[{DateTime.UtcNow}] {requestorEmail}: {email.TextBody}",
                        CreatedOn = DateTime.UtcNow
                    };

                    _context.Tickets.Add(newTicket);
                    await _context.SaveChangesAsync();

                    // Send confirmation email
                    await SendEmailAsync(requestorEmail, "Ticket Created", $"Your ticket #{newTicket.TicketRef} has been created.");

                    // Mark email as read
                    inbox.AddFlags(message.UniqueId, MessageFlags.Seen, true);
                }

                client.Disconnect(true);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing emails: {ex.Message}");
        }
    }

    private int GetIssueTypeIdFromSubject(string subject)
    {
        var issueType = _context.IssueTypes.FirstOrDefault(i => subject.Contains(i.Name));
        return issueType?.Id ?? 1; // Default issue type if not found
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("ARC Helpdesk", "aihelpdesk@arcinternational.co.za"));
            emailMessage.To.Add(new MailboxAddress(toEmail, toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = body };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.yourmailserver.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate("aihelpdesk@arcinternational.co.za", "your-email-password");
                await client.SendAsync(emailMessage);
                client.Disconnect(true);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending email to {toEmail}: {ex.Message}");
        }
    }
}