using Secure_Shipment_Tracker.DTOs;
using Secure_Shipment_Tracker.Models;

namespace Secure_Shipment_Tracker.Repositories.Shipments
{
    public interface IShipmentRepo
    {
        public Task AddShipment(Shipment shipment);
        public Task<User> GetUserById(int id);
        public Task<IEnumerable<Shipment>> GetAllShipments();
        public Task<IEnumerable<Shipment>> GetShipmentsByUserId(int userId);
       public Task<Shipment> GetShipmentByShipId(Guid shipmentId);
        public Task SaveChangesAsync();
    }
}
