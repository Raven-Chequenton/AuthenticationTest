using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class UserDepartment
{
    public string UserId { get; set; }
    public IdentityUser User { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }
}
