namespace Property_Mangement.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Property
    {
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "Owner name is required")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Property type is required")]
        public string PropertyType { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        // Foreign 
        public string ApplicationUserId { get; set; }

        // Navigation 
        public Applicationuser ApplicationUser { get; set; }
    }


}
