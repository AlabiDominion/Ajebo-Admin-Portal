using System.ComponentModel.DataAnnotations;

namespace ShiftSolutions.web.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // Business/application role (not IdentityRole)
    public class BusinessRole
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    
}
