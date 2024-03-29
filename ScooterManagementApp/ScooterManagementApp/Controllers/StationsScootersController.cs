using Microsoft.AspNetCore.Mvc;
using ScooterManagementApp.DAL.Models;
using ScooterManagementApp.DAL.Repositories;
using ScooterManagementApp.Models;

namespace ScooterManagementApp.Controllers
{
    public class StationsScootersController : Controller
    {
        private IStationsScootersRepository _stationsScooRepository;

        public StationsScootersController(IStationsScootersRepository stationsScooRepository)
        {
            _stationsScooRepository = stationsScooRepository;
        }
        public IActionResult ImportFromJson()
        {
            return View(new StationsScootersImportModel());
        }

        [HttpPost]
        public async Task<IActionResult> ImportFromJson(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var rdr = new StreamReader(stream);
            string xml = rdr.ReadToEnd();

            var collection = await _stationsScooRepository.ImportStationsAndScootersFromJson(xml);
            var stationsScooters = new StationsScootersImportModel()
            {
                Scooters = collection.InsertingScooters,
                Stations = collection.InsertedStations
            };
            return View(stationsScooters);
        }

        public IActionResult ImportFromXml()
        {
            return View(new StationsScootersImportModel());
        }

        [HttpPost]
        public async Task<IActionResult> ImportFromXml(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var rdr = new StreamReader(stream);
            string xml = rdr.ReadToEnd();

            var collection = await _stationsScooRepository.ImportStationsAndScootersFromXml(xml);
            var stationsScooters = new StationsScootersImportModel()
            {
                Scooters = collection.InsertingScooters,
                Stations = collection.InsertedStations
            };
            return View(stationsScooters);
        }
    }
}
