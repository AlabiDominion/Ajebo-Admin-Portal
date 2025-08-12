using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ShiftSolutions.web.Data;
using ShiftSolutions.Web.Data;

namespace ShiftSolutions.Web.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var roles = new[] { "SuperAdmin", "Ops", "Finance", "Support", "Reporter" };
            foreach (var r in roles)
                if (!await roleMgr.RoleExistsAsync(r))
                    await roleMgr.CreateAsync(new IdentityRole(r));

            var email = "admin@shift.local";
            var user = await userMgr.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
                await userMgr.CreateAsync(user, "Admin#123"); // change in production
                await userMgr.AddToRoleAsync(user, "SuperAdmin");
            }
        }
    }
}
