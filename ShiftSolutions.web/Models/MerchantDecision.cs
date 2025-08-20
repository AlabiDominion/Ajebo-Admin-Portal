using System.ComponentModel.DataAnnotations;

namespace ShiftSolutions.web.Models
{
    public enum MerchantDecisionType { Approved = 1, Declined = 2 }

    public class MerchantDecision
    {
        public long Id { get; set; }

        [Required, MaxLength(128)]
        public string AgentId { get; set; } = default!;   // from ApartmentsOnLine.AgentId

        public MerchantDecisionType Action { get; set; }

        [MaxLength(2048)]
        public string? Reason { get; set; }               // filled for Declined; optional for Approved

        [MaxLength(128)]
        public string ByUserId { get; set; } = default!;  // Identity user id who performed the action

        public DateTime AtUtc { get; set; } = DateTime.UtcNow;

        // Optional observability / context
        [MaxLength(45)]
        public string? ByIp { get; set; }                 // store client IP if you capture it

        public int AffectedApartments { get; set; }       // how many rows we updated (for agent-level ops)

        // Free-form JSON for future-proofing (requires nvarchar(max))
        public string? MetadataJson { get; set; }         // e.g. { "apartmentIds": [1,2,3] }
    }
}
