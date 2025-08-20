using System.ComponentModel.DataAnnotations;

namespace ShiftSolutions.web.Models
{
    public class Property
    {
        public int Id { get; set; }

        [Required, MaxLength(160)]
        public string Title { get; set; }

        [Required, MaxLength(2000)]
        public string Description { get; set; }

        [Required, MaxLength(256)]
        public string Address { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public PropertyType Type { get; set; } = PropertyType.Shortlet;
        public PropertyStatus Status { get; set; } = PropertyStatus.Pending;

        public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ApprovedDate { get; set; }

        [MaxLength(1024)]
        public string? RejectionReason { get; set; }

        public int Views { get; set; }
        public int Inquiries { get; set; }
        public bool IsConverted { get; set; }
        public DateTime? ConvertedDate { get; set; }

        // FK to merchant (Agent)
        [Required, MaxLength(64)]
        public string AgentId { get; set; }
        public Agents Agent { get; set; }

        // Collections
        public List<string> Images { get; set; } = new();
        public List<PropertyRating> Ratings { get; set; } = new();
    }
}
