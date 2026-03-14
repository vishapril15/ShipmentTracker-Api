using Secure_Shipment_Tracker.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Secure_Shipment_Tracker.DTOs
{
    public class CreateShipmentDto
    {
        [Required]
        public string SenderName { get; set; }
        [Required]
        public string ReceiverName { get; set; }
        [Required]
        public string Origin { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Weight must be greater than 0")]
        public decimal Weight { get; set; }

    }
}
