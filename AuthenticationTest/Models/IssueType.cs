using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationTest.Models
{
    public class IssueType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // ✅ Navigation Property for IssueTypeFields (Fix for your error)
        public virtual ICollection<IssueTypeField> IssueTypeFields { get; set; } = new List<IssueTypeField>();

        public bool RequiresAttachment { get; set; }
    }
}
