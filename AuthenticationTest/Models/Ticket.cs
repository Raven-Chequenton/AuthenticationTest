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
        public string TicketRef { get; set; } = "TEMP"; // ✅ Ensures no NULL value


        // Foreign Key: Assigned User (Engineer)
        [ForeignKey("Assignee")]
        public string? AssigneeId { get; set; }
        public virtual IdentityUser? Assignee { get; set; }

        // Foreign Key: Company
        [Required]
        public int CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        // Foreign Key: Circuit
        public int? CircuitId { get; set; }
        public virtual Circuit? Circuit { get; set; }

        // Foreign Key: IssueType
        [Required]
        public int IssueTypeId { get; set; }
        public virtual IssueType? IssueType { get; set; }

        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        [Required]
        public string ShortDescription { get; set; } = "N/A";

        [Required]
        public string Status { get; set; } = "Unassigned";

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; }

        public string? ProviderRef { get; set; }
        public string CC { get; set; } = "N/A";

        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; } = new List<TicketAttachment>();
        public ICollection<TicketField>? TicketFields { get; set; }

        public string InternalNotesHistory { get; set; } = "N/A";
        public string CustomerCommunicationHistory { get; set; } = "N/A";
        public string RequestorEmail { get; set; } = "N/A";

        public string? SiteName { get; set; }
        public string? VLAN { get; set; }
        
    }
}
