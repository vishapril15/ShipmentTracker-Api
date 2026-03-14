using Secure_Shipment_Tracker.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Secure_Shipment_Tracker.DTOs
{
    public class UpdateShipmentDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public ShipmentStatus NewStatus { get; set; }
    }
}
