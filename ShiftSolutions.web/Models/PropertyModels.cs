
using System.ComponentModel.DataAnnotations;

namespace ShiftSolutions.web.Models
{
    public class PropertyModels
    {
        public enum PropertyStatus
        {
            Pending,
            Approved,
            Rejected
        }

        public enum PropertyType
        {
            Shortlet,
            Lease
        }

        public enum NotificationType
        {
            Approval,
            Rejection,
            Payment,
            Invoice,
            General
        }

        public class Property
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Address { get; set; }
            public decimal Price { get; set; }
            public PropertyType Type { get; set; }
            public PropertyStatus Status { get; set; }
            public DateTime SubmittedDate { get; set; }
            public DateTime? ApprovedDate { get; set; }
            public string? RejectionReason { get; set; }
            public int Views { get; set; }
            public int Inquiries { get; set; }
            public bool IsConverted { get; set; }
            public DateTime? ConvertedDate { get; set; }
            public string AgentId { get; set; }
            public List<string> Images { get; set; } = new List<string>();
            public List<PropertyRating> Ratings { get; set; } = new List<PropertyRating>();
        }

        public class PropertyRating
        {
            public int Id { get; set; }
            public int PropertyId { get; set; }
            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }
            public int Rating { get; set; }
            public string? Review { get; set; }
            public DateTime CreatedDate { get; set; }
            public Property Property { get; set; }
        }

        public class Complaint
        {
            public int Id { get; set; }
            public int PropertyId { get; set; }
            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }
            public string Subject { get; set; }
            public string Description { get; set; }
            public string Status { get; set; } = "Open";
            public DateTime CreatedDate { get; set; }
            public Property Property { get; set; }
        }

        public class Notification
        {
            public int Id { get; set; }
            public string AgentId { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public NotificationType Type { get; set; }
            public bool IsRead { get; set; }
            public DateTime CreatedDate { get; set; }
            public string? RelatedPropertyId { get; set; }
        }


        public class ChangePasswordViewModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string CurrentPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [StringLength(100, MinimumLength = 6)]
            public string NewPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("NewPassword")]
            public string ConfirmPassword { get; set; }
        }

        public class EditProfileViewModel
        {
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            [Phone]
            public string PhoneNumber { get; set; }
            public string? CompanyName { get; set; }
            public string? Address { get; set; }
            public IFormFile? ProfilePicture { get; set; }
        }

        public class BankDetailsViewModel
        {
            [Required]
            public string BankName { get; set; }
            [Required]
            public string AccountNumber { get; set; }
            [Required]
            public string AccountName { get; set; }
            [Required]
            public string BankCode { get; set; }
        }
    }
}
