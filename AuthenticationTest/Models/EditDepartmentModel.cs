using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AuthenticationTest.Models
{
    public class EditDepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> AssignedUserIds { get; set; } = new List<string>();
        public List<UserViewModel> AllUsers { get; set; } = new List<UserViewModel>();
    }
}
