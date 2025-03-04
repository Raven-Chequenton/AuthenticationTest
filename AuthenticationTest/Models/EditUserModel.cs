using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class EditUserModel
{
    public string Id { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string SelectedRole { get; set; }
    public List<string> AvailableRoles { get; set; }

    public int? CompanyId { get; set; } // ✅ Make sure this exists
    public List<Company> AvailableCompanies { get; set; }
}
