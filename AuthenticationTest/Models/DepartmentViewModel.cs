using System.Collections.Generic;

public class DepartmentViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<string> AssignedUsers { get; set; } = new List<string>();
}
