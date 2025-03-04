using System.ComponentModel.DataAnnotations;

namespace AuthenticationTest.Models
{
    public class IssueTypeFieldViewModel
    {
        [Key]
        public int? Id { get; set; } // ✅ Make sure it's nullable (int?) to support HasValue

        [Required]
        public string FieldName { get; set; }

        [Required]
        public string FieldType { get; set; } // Example: "Text", "DateTime"
    }
}
