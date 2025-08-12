using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ShiftSolutions.web.Data
{
    public class ApplicationUser : IdentityUser
    {
        // Add custom fields if needed, e.g. public string DisplayName { get; set; } = "";
    }

    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
