using ScooterManagementApp.DAL.Models;

namespace ScooterManagementApp.Models
{
    public class StationsScootersImportModel
    {
        public IEnumerable<Scooter> Scooters { get; set; } = new List<Scooter>();
        public IEnumerable<Station> Stations { get; set; } = new List<Station>();
    }
}
