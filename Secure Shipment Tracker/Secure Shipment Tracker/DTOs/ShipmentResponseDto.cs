using Secure_Shipment_Tracker.Common.Enums;

namespace Secure_Shipment_Tracker.DTOs
{
    public class ShipmentResponseDto
    {
        public Guid Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string SenderName   { get; set; }
        public string ReceiverName { get; set; }
        public string   Origin {  get; set; }
        public string   Destination { get; set; }
        public decimal Weight { get; set; }
        public string Status { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreatedBy { get; set; }

    }
}
