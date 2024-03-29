using ScooterManagementApp.DAL.Models;

namespace ScooterManagementApp.DAL.Repositories
{
    public interface IStationsScootersRepository
    {
        Task<(IEnumerable<Station> InsertedStations, IEnumerable<Scooter> InsertingScooters)> ImportStationsAndScootersFromJson(string json);
        Task<(IEnumerable<Station> InsertedStations, IEnumerable<Scooter> InsertingScooters)> ImportStationsAndScootersFromXml(string xml);
    }
}
