// Models/MerchantStaff.cs
using System.ComponentModel.DataAnnotations;

namespace ShiftSolutions.web.Models
{
    /// <summary>Join row: one Staff can manage many merchants; one merchant can have many staff.</summary>
    public class MerchantStaff
    {
        public int Id { get; set; }                
        [Required] public int StaffId { get; set; }
        [Required, MaxLength(64)] public string AgentId { get; set; } = "";

        public DateTime AssignedAtUtc { get; set; } = DateTime.UtcNow;
        [MaxLength(64)] public string? AssignedByUserId { get; set; }

        // (optional) navs
        public Staff? Staff { get; set; }
    }
}
