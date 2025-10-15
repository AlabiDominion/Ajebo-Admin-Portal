using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ShiftSolutions.web.Data;  // <-- where ApplicationUser is defined

namespace ShiftSolutions.web.Seeding
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            //using var scope = services.CreateScope();

            //var userManager = scope.ServiceProvider
            //    .GetRequiredService<UserManager<ApplicationUser>>();
            //var roleManager = scope.ServiceProvider
            //    .GetRequiredService<RoleManager<IdentityRole>>();

            //// Seed roles
            //if (!await roleManager.RoleExistsAsync("Admin"))
            //    await roleManager.CreateAsync(new IdentityRole("Admin"));

            //// Seed admin user
            //var admin = await userManager.FindByEmailAsync("admin@shift.local");
            //if (admin == null)
            //{
            //    var newAdmin = new ApplicationUser
            //    {
            //        UserName = "admin@shift.local",
            //        Email = "admin@shift.local",
            //        EmailConfirmed = true
            //    };

            //    await userManager.CreateAsync(newAdmin, "Admin#123");
            //    await userManager.AddToRoleAsync(newAdmin, "Admin");
            //}
        }
    }
}
