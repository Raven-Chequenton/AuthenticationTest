using AuthenticationTest.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class IssueType
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    // ✅ Relationship: One IssueType can have many IssueTypeFields
    public ICollection<IssueTypeField> IssueTypeFields { get; set; } = new List<IssueTypeField>();
}
