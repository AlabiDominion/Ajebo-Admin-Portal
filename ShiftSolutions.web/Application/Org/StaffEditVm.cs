// Application/Org/StaffEditVm.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ShiftSolutions.web.Application.Org
{
    public class StaffEditVm
    {
        [Required]
        public int Id { get; set; }

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
        public int? BusinessRoleId { get; set; }
        [Required, MaxLength(32)]
        public string Status { get; set; } = "Active"; // Active / Inactive / On Leave

        // Dates
        public DateTime? DateJoined { get; set; }

        // Media
        public string? ExistingAvatarUrl { get; set; }
        public IFormFile? Avatar { get; set; }   // optional file upload
    }
}
