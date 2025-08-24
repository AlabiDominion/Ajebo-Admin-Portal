using System.ComponentModel.DataAnnotations;

namespace ShiftSolutions.web.Application.Org
{
    public class DepartmentCreateVm
    {
        [Required, StringLength(80)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class BusinessRoleCreateVm
    {
        [Required, StringLength(80)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class StaffCreateVm
    {
        [Required]
        public string UserId { get; set; } // select an existing ApplicationUser

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int BusinessRoleId { get; set; }

        public string? JobTitle { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
