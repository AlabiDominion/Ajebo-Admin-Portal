using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

using ShiftSolutions.web.Data;          // AppDbContext, ApplicationUser
using ShiftSolutions.web.Services;      // IMerchantService, MerchantService, IPropertyService, PropertyService
using ShiftSolutions.web.Seeding;      // IdentitySeeder

var builder = WebApplication.CreateBuilder(args);

// === DbContext ===
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Ajebos")));

// === Identity ===
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 6;
    opt.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// === Cookies ===
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Auth/Login";
    opt.AccessDeniedPath = "/Auth/Denied";
    opt.Cookie.Name = "ShiftSolutions.Auth";
});

// === MVC + global auth ===
builder.Services.AddControllersWithViews(opt =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});

// === Your domain services ===
builder.Services.AddScoped<IMerchantService, MerchantService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();

var app = builder.Build();

// === Pipeline ===
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// === Routes ===
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// === Seed (optional) ===

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentitySeeder.SeedAsync(services); // uncomment if you have this class
}

app.Run();
