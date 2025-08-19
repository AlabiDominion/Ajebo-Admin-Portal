using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace ShiftSolutions.web.Models
{
    public class Agents
    {
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string UserName { get; set; }
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
        public string? ProfilePicture { get; set; }
        public DateTime JoinedDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        // Bank Details
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? BankCode { get; set; }

        public List<Property> Properties { get; set; } = new List<Property>();
        //public List<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
