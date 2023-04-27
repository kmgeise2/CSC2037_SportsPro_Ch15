using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CSC2037_SportsPro_Ch15.Models
{
    public class Incident
    {
        public int IncidentID { get; set; }

        [Required(ErrorMessage = "Please enter a title.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a description.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter an open date.")]
        public DateTime DateOpened { get; set; } = DateTime.Now;

        public DateTime? DateClosed { get; set; } = null;

        [Required(ErrorMessage = "Please select a customer.")]
        public int CustomerID { get; set; }                   // foreign key property
        [ValidateNever]
        public Customer Customer { get; set; } = null!;       // navigation property

        [Required(ErrorMessage = "Please select a product.")]
        public int ProductID { get; set; }                    // foreign key property
        [ValidateNever]
        public Product Product { get; set; } = null!;         // navigation property

        // initialize to default value for unassigned
        public int TechnicianID { get; set; } = -1;           // foreign key property 
        [ValidateNever]
        public Technician Technician { get; set; } = null!;   // navigation property

    }
}