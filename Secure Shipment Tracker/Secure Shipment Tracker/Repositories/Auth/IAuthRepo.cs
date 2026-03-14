using Secure_Shipment_Tracker.DTOs;
using Secure_Shipment_Tracker.Models;

namespace Secure_Shipment_Tracker.Repositories.Auth
{
    public interface IAuthRepo
    {
        public  Task<User> GetByUsername(string username);
        public Task AddUser(User user);
        public Task<bool> UserExists(string username);
    }
}
