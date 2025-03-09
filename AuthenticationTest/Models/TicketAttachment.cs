using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationTest.Models
{
    public class TicketAttachment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TicketId { get; set; }

        [ForeignKey("TicketId")]
        public virtual Ticket Ticket { get; set; }  // ✅ Ensure correct mapping


        [Required]
        public string FileName { get; set; }

        [Required]
        public string FilePath { get; set; } // Path to the file in the server

        [Required]
        public string UploadedBy { get; set; } // User who uploaded the file

        public DateTime UploadedOn { get; set; } = DateTime.UtcNow;
    }
}
