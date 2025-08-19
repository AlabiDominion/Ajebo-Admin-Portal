using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftSolutions.web.Models
{
    public class ApartmentsOnLine
    {
        public int Id { get; set; }

        [Display(Name = "Apartment Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Apartment Class is required")]
        public string ApartmentClass { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Landmark is required")]
        public string landmark { get; set; }
        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        // Add this property to capture the selected text
        [Display(Name = "State Name")]
        public string StateText { get; set; }

        [Required(ErrorMessage = "Main image is required")]
        public string ImageUrl { get; set; } = "NA";
        [Required(ErrorMessage = "Supporting is required")]
        public string ImageName { get; set; } = "NA";
        [Required(ErrorMessage = "Amenity is required")]
        public string? Amenities { get; set; } = "No";
        [Display(Name = "Common Area")]
        [Required(ErrorMessage = "Common Area is required")]
        public string? CommonArea { get; set; } = "No";
        [Display(Name = "Remote Area")]
        [Required(ErrorMessage = "Remote Area is required")]
        public string RemoteArea { get; set; } = "No";

        [Display(Name = "Urban Area")]
        [Required(ErrorMessage = "Urban Area is required")]
        public string UrbanArea { get; set; } = "No";


        [Required(ErrorMessage = "Availability is required")]
        [Display(Name = "Availability Date")]
        public string Availability { get; set; }
        [Display(Name = "Available Date")]
        [Required(ErrorMessage = "available Date is required")]
        public DateTime AvailableDate { get; set; }
        [Required(ErrorMessage = "Balcony  is required")]
        public string Balcony { get; set; }
        [Required(ErrorMessage = "Maitenance period is required")]
        public string MaintenancePeriod { get; set; }
        [Required(ErrorMessage = "In Maintenance Charges is required")]
        public decimal InMaintenanceCharges { get; set; }
        public bool AgreedToTC { get; set; }
        [Required(ErrorMessage = "In Maintenance Charges is required")]
        public decimal Price { get; set; }
        public decimal TotalTaxes { get; set; }
        public decimal DefaultFare { get; set; }
        [Display(Name = "Delay Charges")]
        [Required(ErrorMessage = "Delay Charges is required")]
        public decimal DelayCharges { get; set; }
        public int Bedrooms { get; set; }
        public string Promotions { get; set; }
        public string InternalService { get; set; } = "NA";

        [Display(Name = "Floors")]
        [Required(ErrorMessage = "Floor number  is required")]
        public int Floors { get; set; }

        [Display(Name = "Bathroom")]
        [Required(ErrorMessage = "Bathroom number is required")]
        public int Bathrooms { get; set; }

        [Required(ErrorMessage = "Furnished Status Charges is required")]
        public string FurnishedStatus { get; set; }
        public int Size { get; set; }
        public string Status { get; set; } = "Pending";

        //public string Type { get; set; }//appartmenttype,right?

        [Display(Name = "Road side")]
        [Required(ErrorMessage = "Road Side number is required")]

        public string Roadside { get; set; }
        public string Agent { get; set; }
        public string AgentId { get; set; }
        //public string StateId { get; set; }
        public string TransactionTime { get; set; } = DateTime.Today.ToString();
        public DateTime CreatedAt { get; set; } = DateTime.Today;
        public DateTime UpdatedAt { get; set; }
        //public State? States { get; set; } 
        [NotMapped]
        public IFormFile[] SupportImage { get; set; }
        public string SupportImage1 { get; set; }
        public string SupportImage2 { get; set; }
        public string SupportImage3 { get; set; }
        public string SupportImage4 { get; set; }

        public decimal Latitude { get; set; }
        public decimal ProfitMargin { get; set; }

        public decimal Longitude { get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
