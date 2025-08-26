namespace ShiftSolutions.web.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string BookingId { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }

        // User Details
        public string ClientName { get; set; } = string.Empty;
        public string ClientEmailAddress { get; set; } = string.Empty;

        // Apartment Details
        public int ApartmentId { get; set; }
        public string ApartmentName { get; set; } = string.Empty;
        public string ApartmentAddress { get; set; } = string.Empty;
        public string ApartmentDescription { get; set; } = string.Empty;
        public string ApartmentCity { get; set; } = string.Empty;
        public decimal ApartmentPricePerNight { get; set; }
        public int ApartmentStarRatings { get; set; }
        public string ApartmentStayDuration { get; set; } = string.Empty;

        // Payment Details
        public string TransactionId { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string NumberOfNights { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string TransactionReferenceId { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;

        // Tax Details
        public decimal VatPercentage { get; set; }
        public decimal Vat { get; set; }
        public decimal SubTotal { get; set; }

        // Audit Fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
