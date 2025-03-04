using AuthenticationTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationTest.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<Circuit> Circuits { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<UserDepartment> UserDepartments { get; set; } // ✅ Add this to ensure User-Department relationship
        public DbSet<IssueType> IssueTypes { get; set; }
        public DbSet<IssueTypeField> IssueTypeFields { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ✅ Prevent data loss when migrating
            builder.Entity<IdentityRole>()
                .ToTable("AspNetRoles", t => t.ExcludeFromMigrations());

            builder.Entity<IdentityUserRole<string>>()
                .ToTable("AspNetUserRoles", t => t.ExcludeFromMigrations());

            builder.Entity<UserCompany>()
                .ToTable("UserCompanies", t => t.ExcludeFromMigrations());

            // ✅ Define relationship between Users and Companies
            builder.Entity<UserCompany>()
                .HasKey(uc => new { uc.UserId, uc.CompanyId });

            builder.Entity<UserCompany>()
                .HasOne(uc => uc.User)
                .WithMany()
                .HasForeignKey(uc => uc.UserId);

            builder.Entity<UserCompany>()
                .HasOne(uc => uc.Company)
                .WithMany(c => c.UserCompanies)
                .HasForeignKey(uc => uc.CompanyId);

            // ✅ Define relationship between Users and Departments
            builder.Entity<UserDepartment>()
                .HasKey(ud => new { ud.UserId, ud.DepartmentId });

            builder.Entity<UserDepartment>()
                .HasOne(ud => ud.User)
                .WithMany()
                .HasForeignKey(ud => ud.UserId);

            builder.Entity<UserDepartment>()
                .HasOne(ud => ud.Department)
                .WithMany(d => d.UserDepartments)
                .HasForeignKey(ud => ud.DepartmentId);

            // ✅ Define Relationship: IssueType ↔ IssueTypeField
            builder.Entity<IssueTypeField>()
                .HasOne(f => f.IssueType)
                .WithMany(i => i.IssueTypeFields)
                .HasForeignKey(f => f.IssueTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Seed default roles
            string[] roles = { "Admin", "Client", "Agent" };
            foreach (var role in roles)
            {
                builder.Entity<IdentityRole>().HasData(new IdentityRole
                {
                    Name = role,
                    NormalizedName = role.ToUpper()
                });
            }
        }
    }
}
