using Secure_Shipment_Tracker.DTOs;
using Secure_Shipment_Tracker.Models;

namespace Secure_Shipment_Tracker.Services.Shipments
{
    public interface IShipmentService
    {
        public  Task<ShipmentResponseDto> AddShipment(CreateShipmentDto shipmentDto,int createdBy);
        public Task<IEnumerable<ShipmentResponseDto>> GetAllShipmentsOfAdmin();
        public Task<IEnumerable<ShipmentResponseDto>> GetShipmentsByUser(int userId);
        public Task<Shipment> UpdateShipmentStatus(UpdateShipmentDto dto,string role,int id);
    }
}
