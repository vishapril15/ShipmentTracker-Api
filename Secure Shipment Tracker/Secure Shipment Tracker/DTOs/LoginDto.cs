using System.ComponentModel.DataAnnotations;

namespace Secure_Shipment_Tracker.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
