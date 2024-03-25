namespace ScooterManagementApp.DAL.Models
{
    public class Scooter
    {
        public int ScooterId { get; set; }
        public int StationId { get; set; }
        public required string Model { get; set; }
        public decimal BatteryCapacity { get; set; }
        public int MaxSpeed { get; set; }
        public required string Status { get; set; }
    }
}
