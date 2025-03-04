using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationTest.Models
{
    public class EditIssueTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<IssueTypeFieldViewModel> Fields { get; set; } = new List<IssueTypeFieldViewModel>();
    }
}
