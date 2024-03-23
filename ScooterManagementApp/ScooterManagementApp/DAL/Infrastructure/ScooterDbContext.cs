using Microsoft.EntityFrameworkCore;
using ScooterManagementApp.DAL.Models;

namespace ScooterManagementApp.DAL.Infrastructure
{
    public class ScooterDbContext : DbContext
    {
        public ScooterDbContext(DbContextOptions options) : base(options) { }

        protected ScooterDbContext()
        {
            
        }

        public DbSet<Station> Stations { get; set; }
        public DbSet<Scooter> Scooters { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }


    }

}
