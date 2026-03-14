using Secure_Shipment_Tracker.Common.Enums;

namespace Secure_Shipment_Tracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
    }
}
