using Dapper;
using Microsoft.Data.SqlClient;
using ScooterManagementApp.DAL.Models;
using System.Data;

namespace ScooterManagementApp.DAL.Repositories
{
    public class StationsScootersReporitory : IStationsScootersRepository
    {
        private readonly string _connStr;

        public StationsScootersReporitory(string connStr)
        {
            _connStr = connStr;
        }

        public async Task<(IEnumerable<Station> InsertedStations, IEnumerable<Scooter> InsertingScooters)> ImportStationsAndScootersFromJson(string json)
        {
            using var conn = new SqlConnection(_connStr);

            var parameters = new DynamicParameters();
            parameters.Add("@json", json);

            using var multi = await conn.QueryMultipleAsync("udpScooterStationImportFromJson", parameters, commandType: CommandType.StoredProcedure);

            var insertedStations = await multi.ReadAsync<Station>();
            var insertingScooters = await multi.ReadAsync<Scooter>();

            return (insertedStations, insertingScooters);
        }

        public async Task<(IEnumerable<Station> InsertedStations, IEnumerable<Scooter> InsertingScooters)> ImportStationsAndScootersFromXml(string xml)
        {
            using var conn = new SqlConnection(_connStr);

            var parameters = new DynamicParameters();
            parameters.Add("@xml", xml);

            using var multi = await conn.QueryMultipleAsync("udpScooterStationImportFromXml", parameters, commandType: CommandType.StoredProcedure);

            var insertedStations = await multi.ReadAsync<Station>();
            var insertingScooters = await multi.ReadAsync<Scooter>();

            return (insertedStations, insertingScooters);
        }
    }
}
