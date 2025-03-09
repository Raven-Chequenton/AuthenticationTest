using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Circuit
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string CircuitID { get; set; }

    [Required]
    public string SiteName { get; set; }

    [Required]
    public string VLAN { get; set; }

    [Required]
    public int? SLA { get; set; } // Stored as hours (e.g., "4" for 4-hour SLA)

    [Required(ErrorMessage = "Please select a company.")]
    [Display(Name = "Company")]
    public int CompanyId { get; set; }

    [ForeignKey("CompanyId")]
    public virtual Company? Company { get; set; } // ✅ Ensure this is correctly defined

    [Required]
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow; // Auto-set when created

    
    public string? CreatedBy { get; set; } // Stores Name and Surname of creator
}
