using System.ComponentModel.DataAnnotations;

namespace ShiftSolutions.web.Models
{
    public class PropertyImage
    {
        public int Id { get; set; }

        public int PropertyId { get; set; }
        public Property Property { get; set; }

        [Required, MaxLength(512)]
        public string BlobKey { get; set; }   // path/key in your cloud storage

        [MaxLength(256)]
        public string? FileName { get; set; }

        public int SortOrder { get; set; } = 0;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
