using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSC2037_SportsPro_Ch15.Models
{
    public class Product
    {
        public Product() => Customers = new HashSet<Customer>();

        public int ProductID { get; set; }

        [Required(ErrorMessage = "Please enter a product code.")]
        public string ProductCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a product name.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a yearly price.")]
        [Range(1, 1000000, ErrorMessage = "Yearly price must be greater than zero.")]
        [Column(TypeName = "decimal(8,2)")]
        [Display(Name = "Yearly Price")]
        public decimal YearlyPrice { get; set; }

        [Required(ErrorMessage = "Please enter a release date.")]
        public DateTime ReleaseDate { get; set; } = DateTime.Now;

        public ICollection<Customer> Customers { get; set; }   // skip navigation property for many-to-many relationship
    }
}
