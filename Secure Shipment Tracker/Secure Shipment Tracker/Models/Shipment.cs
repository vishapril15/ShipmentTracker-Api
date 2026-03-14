using Secure_Shipment_Tracker.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secure_Shipment_Tracker.Models
{
    public class Shipment
    {
        public Guid Id { get; set; }
       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShipmentNumber { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string Origin    { get; set; }
        public string Destination { get; set; }
        public decimal Weight { get; set; }
        public ShipmentStatus Status { get; set; } = ShipmentStatus.Created;
        public DateTime CreatedAt { get; set; }
     
        [ForeignKey("User")]
        public int CreatedBy { get; set; }
        public User User { get; set; }

    }
}
