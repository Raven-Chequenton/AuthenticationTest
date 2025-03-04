using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class UserCompany
{
    public string UserId { get; set; }
    public IdentityUser User { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; }
}
