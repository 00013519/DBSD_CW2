namespace ScooterManagementApp.DAL.Models
{
    public class Station
    {
        public int StationId { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public int CurrentAmount { get; set; }

        // Navigation property for the Scooters
        public ICollection<Scooter> Scooters { get; set; }
    }

}
