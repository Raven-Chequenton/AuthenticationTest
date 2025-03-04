using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationTest.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; } // Internal ID for database use

        [Required]
        [StringLength(20)]
        public string TicketRef { get; set; } // Auto-generated in format "ARC-00000x"

        [Required]
        public string Requestor { get; set; } // Name + Surname of the registered user

        [Required]
        public string ShortDescription { get; set; } // IssueType + CircuitID

        [ForeignKey("Company")]
        public int? CompanyId { get; set; } // Nullable in case the requestor isn't in the system
        public Company? Company { get; set; }

        [ForeignKey("AssignedUser")]
        public string? AssignedUserId { get; set; } // Nullable, assigned later
        public IdentityUser? AssignedUser { get; set; } // ✅ FIX: Replace ApplicationUser with IdentityUser

        [Required]
        public DateTime CreatedOn { get; set; } // Timestamp when ticket was created

        public DateTime? UpdatedOn { get; set; } // Timestamp when ticket was last updated
    }
}
