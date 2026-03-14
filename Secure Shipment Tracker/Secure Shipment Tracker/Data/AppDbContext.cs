using Microsoft.EntityFrameworkCore;
using Secure_Shipment_Tracker.Models;

namespace Secure_Shipment_Tracker.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Shipment> Shipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shipment>()
                .Property(s => s.ShipmentNumber)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Shipment>().
                HasIndex(s => s.ShipmentNumber)
                .IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}
