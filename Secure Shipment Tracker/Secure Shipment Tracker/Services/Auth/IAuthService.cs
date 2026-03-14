using Secure_Shipment_Tracker.DTOs;

namespace Secure_Shipment_Tracker.Services.Auth
{
    public interface IAuthService
    {
        public Task<LoginResponsedto> Login(LoginDto dto);
        public Task Register(RegisterDto dto);
    }
}
