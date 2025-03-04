using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AuthenticationTest.Models
{
    public class EditDepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> SelectedUserIds { get; set; } = new List<string>(); // ✅ Store selected user IDs
        public List<ApplicationUser> AvailableUsers { get; set; } = new List<ApplicationUser>(); // ✅ Store users
    }
}
