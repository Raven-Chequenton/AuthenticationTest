using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuthenticationTest.Data;
using AuthenticationTest.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ✅ Register Email Service
//builder.Services.AddScoped<IEmailService, EmailService>();


// ✅ Microsoft Graph Service for Emails
builder.Services.AddSingleton<GraphAuth>();

// ✅ Ensure Background Services (if needed)
//builder.Services.AddHostedService<EmailProcessingService>(); // Background service for processing emails
//builder.Services.AddHostedService<SLAService>(); // SLA Tracking

// ✅ Add Identity with Roles
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ✅ Ensure roles & default admin user are created on startup
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    string[] roles = { "Admin", "Client", "Agent" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // ✅ Ensure Admin User exists
    string adminEmail = "admin@example.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser != null)
    {
        var rolesAssigned = await userManager.GetRolesAsync(adminUser);
        if (!rolesAssigned.Contains("Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    // ✅ Ensure User-Company assignments persist
    var users = userManager.Users.ToList();
    var companies = dbContext.Companies.ToList();
    var existingUserCompanies = dbContext.UserCompanies.ToList();

    foreach (var user in users)
    {
        var userCompany = existingUserCompanies.FirstOrDefault(uc => uc.UserId == user.Id);
        if (userCompany == null && companies.Any())
        {
            dbContext.UserCompanies.Add(new UserCompany
            {
                UserId = user.Id,
                CompanyId = companies.First().Id // Assign first company as default
            });
        }
    }

    await dbContext.SaveChangesAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
