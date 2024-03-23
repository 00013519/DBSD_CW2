namespace ScooterManagementApp.DAL.Models
{
    public class Scooter
    {
        public int ScooterId { get; set; }
        public int StationId { get; set; }  // Foreign Key
        public Station Station { get; set; } // Navigation property
        public string Model { get; set; }
        public decimal BatteryLevel { get; set; }
        public string Status { get; set; }
    }

}
