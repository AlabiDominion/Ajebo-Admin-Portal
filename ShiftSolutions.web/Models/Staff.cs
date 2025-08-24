using System.ComponentModel.DataAnnotations;

namespace ShiftSolutions.web.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string UserId { get; set; } = "";

        // Personal
        [Required, MaxLength(80)]
        public string FirstName { get; set; } = "";
        [Required, MaxLength(80)]
        public string LastName { get; set; } = "";
        [Required, EmailAddress, MaxLength(160)]
        public string Email { get; set; } = "";
        [MaxLength(40)]
        public string? Phone { get; set; }

        // Org
        public int? DepartmentId { get; set; }
        public int? RoleId { get; set; }
        public string Status { get; set; } = "Active"; // Active / Inactive / On Leave

        // Dates
        public DateTime? DateJoined { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Media
        [MaxLength(400)]
        public string? AvatarUrl { get; set; }

        // (Optional) lightweight account fields (NOT Identity – you can ignore for now)
        [MaxLength(80)]
        public string? Username { get; set; }
        [MaxLength(400)]
        public string? PasswordHash { get; set; }  // store a hash if you must; or leave null

        // Convenience
        public string FullName => $"{FirstName} {LastName}";
    }
}
