using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShiftSolutions.web.Models;

namespace ShiftSolutions.web.Data
{
    public class ApplicationUser : IdentityUser { }

    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets
        public DbSet<Agents> Agents { get; set; }
        public DbSet<ApartmentsOnLine> Apartments { get; set; }
        public DbSet<Property> Properties { get; set; }                 
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<PropertyRating> PropertyRatings { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<MerchantDecision> MerchantDecisions { get; set; } = default!;
        public DbSet<Department> Departments { get; set; }
        public DbSet<BusinessRole> BusinessRoles { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<MerchantStaff> MerchantStaff { get; set; } = default!;
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Department>()
        .HasIndex(x => x.Name).IsUnique();

            builder.Entity<BusinessRole>()
                .HasIndex(x => x.Name).IsUnique();

            builder.Entity<Staff>(e =>
            {
                e.HasIndex(x => x.Email);
                e.HasIndex(x => x.Username);
                e.Property(x => x.Status).HasMaxLength(32);
                e.Property(x => x.FirstName).HasMaxLength(80).IsRequired();
                e.Property(x => x.LastName).HasMaxLength(80).IsRequired();
                e.Property(x => x.Email).HasMaxLength(160).IsRequired();
                e.Property(x => x.AvatarUrl).HasMaxLength(400);
                e.Property(x => x.Username).HasMaxLength(80);
                e.Property(x => x.PasswordHash).HasMaxLength(400);
            });
            builder.Entity<MerchantStaff>(e =>
            {
                e.Property(x => x.AgentId).HasMaxLength(64).IsRequired();

                // A staff can be linked to a given merchant only once.
                e.HasIndex(x => new { x.StaffId, x.AgentId }).IsUnique();

                // (optional) FK for Staff
                e.HasOne(ms => ms.Staff)
                 .WithMany()                 // add a collection on Staff if you want: .WithMany(s => s.Merchants)
                 .HasForeignKey(ms => ms.StaffId)
                 .OnDelete(DeleteBehavior.Cascade);
            });


            // === Agents ===
            builder.Entity<Agents>(e =>
            {
                e.HasKey(a => a.Id);
                e.Property(a => a.Id).HasMaxLength(64);
                e.HasIndex(a => a.Email);
                e.HasIndex(a => a.UserName);
                e.HasIndex(a => a.CreatedAt);
                // comment out next two if you haven't added these fields yet
                // e.HasIndex(a => a.ApprovalStatus);
                // e.HasIndex(a => a.City);
            });

            // === Properties (listings) ===
            builder.Entity<Property>(e =>
            {
                e.ToTable("Properties");
                e.HasKey(p => p.Id);
                e.Property(p => p.AgentId).IsRequired().HasMaxLength(64);

                e.HasOne(p => p.Agent)
                 .WithMany(a => a.Properties)
                 .HasForeignKey(p => p.AgentId)
                 .HasPrincipalKey(a => a.Id)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(p => p.Status);
                e.HasIndex(p => p.SubmittedDate);
            });

            // === PropertyImages ===
            builder.Entity<PropertyImage>(e =>
            {
                e.ToTable("PropertyImages");
                e.HasKey(pi => pi.Id);
                e.HasIndex(pi => new { pi.PropertyId, pi.SortOrder });
            });

            // === PropertyRatings ===
            builder.Entity<PropertyRating>(e =>
            {
                e.ToTable("PropertyRatings");
                e.HasKey(r => r.Id);
                e.HasIndex(r => new { r.PropertyId, r.CreatedDate });
            });

            // === Complaints ===
            builder.Entity<Complaint>(e =>
            {
                e.ToTable("Complaints");
                e.HasKey(c => c.Id);
                e.HasIndex(c => new { c.PropertyId, c.CreatedDate });
            });

            // === Notifications ===
            builder.Entity<Notification>(e =>
            {
                e.ToTable("Notifications");
                e.HasKey(n => n.Id);
                e.Property(n => n.AgentId).HasMaxLength(64);
                e.HasOne(n => n.Agent)
                 .WithMany() // add List<Notification> on Agents if you want
                 .HasForeignKey(n => n.AgentId)
                 .HasPrincipalKey(a => a.Id)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // === Legacy ApartmentsOnLine (optional) ===
            builder.Entity<ApartmentsOnLine>(e =>
            {
                e.HasKey(x => x.Id);
            });

            builder.Entity<MerchantDecision>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.AgentId).IsRequired().HasMaxLength(128);
                e.Property(x => x.ByUserId).HasMaxLength(128);
                e.Property(x => x.Reason).HasMaxLength(2048);
                e.Property(x => x.ByIp).HasMaxLength(45);

                e.HasIndex(x => x.AgentId);
                e.HasIndex(x => new { x.AgentId, x.AtUtc });
                e.HasIndex(x => x.AtUtc);
            });
        }
    }
}
