
using Microsoft.EntityFrameworkCore;
using Secure_Shipment_Tracker.Data;
using Secure_Shipment_Tracker.Models;

namespace Secure_Shipment_Tracker.Repositories.Shipments
{
    public class ShipmentRepo : IShipmentRepo
    {
        private readonly AppDbContext _context;
        public ShipmentRepo(AppDbContext cont)
        {
            _context = cont;
        }
        public async Task AddShipment(Shipment shipment)
        {
           await _context.Shipments.AddAsync(shipment);
           await _context.SaveChangesAsync();      
        }

        public async Task<IEnumerable<Shipment>> GetAllShipments()
        {
            return await _context.Shipments.Include(s=>s.User).ToListAsync();
        }

        public async Task<Shipment> GetShipmentByShipId(Guid shipmentId)
        {
            return await _context.Shipments.FirstOrDefaultAsync(s=> s.Id == shipmentId);
        }

        public async Task<IEnumerable<Shipment>> GetShipmentsByUserId(int userId)
        {
            //return await _context.Shipments.Include(s => s.User)
            //    .Where(u => u.User.Id == userId).ToListAsync();
            return await _context.Shipments
                .Include(s=>s.User)
                .Where(s => s.CreatedBy == userId).ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
           return await _context.Users.FirstOrDefaultAsync(u=>u.Id == id);      
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
