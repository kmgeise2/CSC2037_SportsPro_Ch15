using System.ComponentModel.DataAnnotations;

namespace CSC2037_SportsPro_Ch15.Models
{
    public class Technician
    {
        public int TechnicianID { get; set; }

        [Required(ErrorMessage = "Please enter a name.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter an email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a phone.")]
        public string Phone { get; set; } = string.Empty;
    }
}
