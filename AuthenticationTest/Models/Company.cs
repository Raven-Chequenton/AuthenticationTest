using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Company
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    // ✅ Relationship to Users via UserCompany
    public ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
}
