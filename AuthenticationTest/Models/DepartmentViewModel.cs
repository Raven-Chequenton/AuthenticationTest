using AuthenticationTest.Models;
using System.Collections.Generic;


public class DepartmentViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<UserViewModel> AssignedUsers { get; set; } = new List<UserViewModel>(); // ✅ Use UserViewModel

}
