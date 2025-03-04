using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationTest.Models
{
    public class IssueTypeField
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FieldName { get; set; }  // ✅ Field Name (e.g., "Onsite Contact")

        [Required]
        [StringLength(50)]
        public string FieldType { get; set; }  // ✅ Field Type (Text, DateTime, etc.)

        // ✅ Foreign Key to IssueType
        [ForeignKey("IssueType")]
        public int IssueTypeId { get; set; }
        public IssueType? IssueType { get; set; } // ✅ Nullable to prevent errors when unassigned
    }
}
