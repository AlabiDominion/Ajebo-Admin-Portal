using System.ComponentModel.DataAnnotations;

namespace ShiftSolutions.web.Application.Org
{
    public class StaffCreateWithUserVm
    {
        // Identity user
        [Required] public string FirstName { get; set; } = "";
        [Required] public string LastName { get; set; } = "";
        [Required, EmailAddress] public string Email { get; set; } = "";
        [Required, Phone] public string PhoneNumber { get; set; } = "";
        [Required] public string UserName { get; set; } = "";
        [Required, MinLength(6)] public string Password { get; set; } = "";
        [Compare(nameof(Password))] public string ConfirmPassword { get; set; } = "";

        // Staff fields
        [Required] public int DepartmentId { get; set; }
        [Required] public int BusinessRoleId { get; set; }
        public string? JobTitle { get; set; }
        public DateTime? DateJoined { get; set; }
        public bool IsActive { get; set; } = true;
        public IFormFile? Avatar { get; set; }
    }
}
