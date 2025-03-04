using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AuthenticationTest.Models;

public class IssueTypeViewModel
{
    [Required]
    public string Name { get; set; }

    public List<IssueTypeFieldViewModel> Fields { get; set; } = new List<IssueTypeFieldViewModel>();
}


