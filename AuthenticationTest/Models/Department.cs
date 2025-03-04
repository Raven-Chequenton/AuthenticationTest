using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class Department
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    // ✅ Many-to-Many Relationship: Department ↔ Users
    public ICollection<UserDepartment> UserDepartments { get; set; } = new List<UserDepartment>();

}
