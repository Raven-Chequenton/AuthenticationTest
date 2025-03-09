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
        public DbSet<UserDepartment> UserDepartments { get; set; } // ✅ Ensures User-Department mapping
        public DbSet<IssueType> IssueTypes { get; set; }
        public DbSet<IssueTypeField> IssueTypeFields { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }
        public DbSet<TicketField> TicketFields { get; set; } // ✅ Ensures Ticket Fields exist

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ✅ Exclude Identity tables from migrations
            builder.Entity<IdentityRole>().ToTable("AspNetRoles", t => t.ExcludeFromMigrations());
            builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles", t => t.ExcludeFromMigrations());
            builder.Entity<UserCompany>().ToTable("UserCompanies", t => t.ExcludeFromMigrations());

            // ✅ Define User-Company Relationship
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

            // ✅ Define User-Department Relationship
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

            // ✅ Define Relationships for Ticket Table
             // ❌ Prevents cascade delete

            builder.Entity<Ticket>()
                .HasOne(t => t.Assignee)
                .WithMany()
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict); // ❌ Prevents cascade delete

            builder.Entity<Ticket>()
                .HasOne(t => t.Company)
                .WithMany()
                .HasForeignKey(t => t.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ticket>()
                .HasOne(t => t.Circuit)
                .WithMany()
                .HasForeignKey(t => t.CircuitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ticket>()
                .HasOne(t => t.IssueType)
                .WithMany()
                .HasForeignKey(t => t.IssueTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Add missing relationships
            builder.Entity<Ticket>()
                .HasOne(t => t.AssignedUser) // 🔹 Assigned User (new relationship)
                .WithMany()
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.Restrict); // ❌ Prevents cascade delete

            builder.Entity<Ticket>()
                .HasOne(t => t.Department) // 🔹 Department (new relationship)
                .WithMany()
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict); // ❌ Prevents cascade delete

            // ✅ Define Ticket Attachments Relationship
            builder.Entity<TicketAttachment>()
                .HasOne(ta => ta.Ticket)
                .WithMany(t => t.TicketAttachments)
                .HasForeignKey(ta => ta.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Define Ticket Fields Relationship
            builder.Entity<TicketField>()
                .HasOne(tf => tf.Ticket)
                .WithMany(t => t.TicketFields)
                .HasForeignKey(tf => tf.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Seed Default Roles
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
