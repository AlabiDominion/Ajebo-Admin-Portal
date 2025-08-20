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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
