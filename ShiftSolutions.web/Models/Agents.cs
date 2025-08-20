using System.ComponentModel.DataAnnotations;

namespace ShiftSolutions.web.Models
{
    public enum MerchantApprovalStatus
    {
        Pending = 0,
        Approved = 1,
        Declined = 2
    }

    public class Agents
    {
        [Key]
        [MaxLength(64)]
        public string Id { get; set; }

        [Required, MaxLength(128)]
        public string FirstName { get; set; }

        [Required, MaxLength(128)]
        public string LastName { get; set; }

        [Required, MaxLength(128)]
        public string UserName { get; set; }

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; }

        [Required, Phone, MaxLength(32)]
        public string PhoneNumber { get; set; }

        [MaxLength(256)]
        public string? CompanyName { get; set; }

        [MaxLength(256)]
        public string? Address { get; set; }

        // Optional: store city separately so we can filter/sort easily in UI
        [MaxLength(128)]
        public string? City { get; set; }

        [MaxLength(512)]
        public string? ProfilePicture { get; set; }

        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // ------------ Bank Details ------------
        [MaxLength(128)]
        public string? BankName { get; set; }

        [MaxLength(32)]
        public string? AccountNumber { get; set; }

        [MaxLength(128)]
        public string? AccountName { get; set; }

        [MaxLength(32)]
        public string? BankCode { get; set; }

        // ------------ Merchant approval (one-time KYC) ------------
        public string ApprovalStatus { get; set; } = "Pending";

        public DateTime? ApprovedAtUtc { get; set; }
        public DateTime? DeclinedAtUtc { get; set; }

        [MaxLength(1024)]
        public string? DeclinedReason { get; set; }
        public string? ApprovedById { get; set; }
        

        [MaxLength(128)]
        public string? ReviewedByUserId { get; set; }

        // ------------ Navigation ------------
        public List<Property> Properties { get; set; } = new();
    }
}
