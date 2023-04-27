using Microsoft.AspNetCore.Mvc;                          // for Remote attribute
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;  // for ValidateNever attribute
using System.ComponentModel.DataAnnotations;

namespace CSC2037_SportsPro_Ch15.Models
{
    public class Customer
    {
        public Customer() => Products = new HashSet<Product>();

        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Please enter a first name.")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a last name.")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter an address.")]
        [StringLength(50)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a city.")]
        [StringLength(50)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a state.")]
        [StringLength(50)]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a postal code.")]
        [StringLength(20)]
        public string PostalCode { get; set; } = string.Empty;

        [RegularExpression(@"^((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}$",
            ErrorMessage = "Phone number must be in (999) 999-9999 format.")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [Remote("CheckEmail", "Validation", AdditionalFields = "CustomerID")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please select a country.")]
        public string CountryID { get; set; } = string.Empty;   // foreign key property
        [ValidateNever]
        public Country Country { get; set; } = null!;           // navigation property

        public string FullName => FirstName + " " + LastName;   // read-only property

        public ICollection<Product> Products { get; set; }      // skip navigation property for many-to-many relationship
    }
}