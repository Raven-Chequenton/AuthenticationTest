using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationTest.Models
{
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey("Company")]
        public int? CompanyId { get; set; } // ✅ This links users to a company
        public Company? Company { get; set; }
    }
}
