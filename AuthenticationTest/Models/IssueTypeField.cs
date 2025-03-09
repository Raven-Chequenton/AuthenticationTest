using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationTest.Models
{
    public class IssueTypeField
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FieldName { get; set; }

        [Required]
        public string FieldType { get; set; } // (e.g., Text, DateTime, Dropdown)

        // ✅ Foreign Key to IssueType
        public int IssueTypeId { get; set; }
        [ForeignKey("IssueTypeId")]
        public virtual IssueType IssueType { get; set; }
    }
}
