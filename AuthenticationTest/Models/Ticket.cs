using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationTest.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TicketRef { get; set; } // ✅ Auto-generated ref (ARC-00000x)

        // ✅ Foreign Key: Requestor (User who created the ticket)
        //[Required]
        //[ForeignKey("Requestor")]
        //public string RequestorId { get; set; }
        //public virtual IdentityUser Requestor { get; set; }

        public string RequestorUsername { get; set; }

        // ✅ Foreign Key: Assigned User (Engineer)
        [ForeignKey("Assignee")]
        public string? AssigneeId { get; set; }
        public virtual IdentityUser? Assignee { get; set; }

        public string AssignedUserId { get; set; } // ✅ Ensure this is a string
        public virtual IdentityUser AssignedUser { get; set; } // ✅ Reference IdentityUser

        // ✅ Foreign Key: Company
        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        // ✅ Foreign Key: Circuit
        public int? CircuitId { get; set; }
        public virtual Circuit Circuit { get; set; }

        // ✅ Foreign Key: IssueType
        [Required]
        public int IssueTypeId { get; set; }
        public virtual IssueType IssueType { get; set; }

        public int? DepartmentId { get; set; } // ✅ Fix for your error
        public virtual Department Department { get; set; }

    

        [Required]
        public string ShortDescription { get; set; } = ""; // ✅ Ensure it is not null

        [Required]
        public string Status { get; set; } // ✅ (Unassigned, Open, Pending, etc.)

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; }

        public string? ProviderRef { get; set; }
        public string? SLA { get; set; }
        public string? CC { get; set; }

        // ✅ Navigation Property for Ticket Fields (Fix for your error)
       

        // ✅ Navigation Property for Attachments
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; } = new List<TicketAttachment>(); // ✅ Correct mapping

        public ICollection<TicketField> TicketFields { get; set; }

        // ✅ Internal Notes & Customer Communication History Fields
        public string? InternalNotesHistory { get; set; }
        public string? CustomerCommunicationHistory { get; set; }
        public string? RequestorEmail { get; internal set; }
        
    }
}
