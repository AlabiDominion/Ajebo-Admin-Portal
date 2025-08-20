using System.ComponentModel.DataAnnotations;

namespace ShiftSolutions.web.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required, MaxLength(64)]
        public string AgentId { get; set; }   // who to notify (merchant)
        public Agents Agent { get; set; }

        [Required, MaxLength(160)]
        public string Title { get; set; }

        [Required, MaxLength(2000)]
        public string Message { get; set; }

        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(32)]
        public string? RelatedPropertyId { get; set; } // keep string if you intend cross‑entity IDs; otherwise int?
    }
}
