using System.ComponentModel.DataAnnotations;

namespace ShiftSolutions.web.Models
{
    public class PropertyRating
    {
        public int Id { get; set; }

        public int PropertyId { get; set; }
        public Property Property { get; set; }

        [Required, MaxLength(128)]
        public string CustomerName { get; set; }

        [Required, EmailAddress, MaxLength(256)]
        public string CustomerEmail { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(2000)]
        public string? Review { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
