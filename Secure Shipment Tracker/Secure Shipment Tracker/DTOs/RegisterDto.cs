using Secure_Shipment_Tracker.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Secure_Shipment_Tracker.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Username {  get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public UserRole Role {  get; set; }
    }
}
