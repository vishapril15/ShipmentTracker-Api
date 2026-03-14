namespace Secure_Shipment_Tracker.DTOs
{
    public class LoginResponsedto
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public DateTime Expiry { get; set; }
    }
}
