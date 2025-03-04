using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

public class UserManagementViewModel
{
    public List<IdentityUser> Users { get; set; }
    public List<Company> Companies { get; set; }

    // ✅ Dictionary to store user roles
    public Dictionary<string, string> UserRoles { get; set; } = new Dictionary<string, string>();

    // ✅ Dictionary to map users to their companies
    public Dictionary<string, string> UserCompanies { get; set; } = new Dictionary<string, string>();
}
