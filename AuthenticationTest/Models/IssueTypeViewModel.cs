using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class IssueTypeViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public List<IssueTypeFieldViewModel> Fields { get; set; } = new List<IssueTypeFieldViewModel>();
}

public class IssueTypeFieldViewModel
{
    public int Id { get; set; }

    [Required]
    public string FieldName { get; set; }

    [Required]
    public string FieldType { get; set; }
}
