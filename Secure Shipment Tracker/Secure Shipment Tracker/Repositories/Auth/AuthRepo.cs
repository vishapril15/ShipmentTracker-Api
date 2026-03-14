using Microsoft.EntityFrameworkCore;
using Secure_Shipment_Tracker.Data;
using Secure_Shipment_Tracker.DTOs;
using Secure_Shipment_Tracker.Models;
using BCrypt.Net;
using Secure_Shipment_Tracker.Helpers;

namespace Secure_Shipment_Tracker.Repositories.Auth
{
    public class AuthRepo : IAuthRepo
    {
        private readonly AppDbContext _context;
        public AuthRepo(AppDbContext cont)
        {
            _context = cont;
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            _context.SaveChanges();
        }

        public async Task<User> GetByUsername(string username)
        {      
              return await _context.Users.FirstOrDefaultAsync(d=> d.UserName== username); 
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(u=> u.UserName == username);
        }
    }
}
