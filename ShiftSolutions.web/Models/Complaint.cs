using System.ComponentModel.DataAnnotations;

namespace ShiftSolutions.web.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        public int PropertyId { get; set; }
        public Property Property { get; set; }

        [Required, MaxLength(128)]
        public string CustomerName { get; set; }

        [Required, EmailAddress, MaxLength(256)]
        public string CustomerEmail { get; set; }

        [Required, MaxLength(160)]
        public string Subject { get; set; }

        [Required, MaxLength(4000)]
        public string Description { get; set; }

        [Required, MaxLength(48)]
        public string Status { get; set; } = "Open";

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
