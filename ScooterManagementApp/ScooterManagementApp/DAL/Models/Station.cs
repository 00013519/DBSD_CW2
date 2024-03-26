namespace ScooterManagementApp.DAL.Models
{
    public class Station
    {
        public int StationId { get; set; }
        public required string City { get; set; }
        public required string District { get; set; }
        public required string Street { get; set; }
        public int Capacity { get; set; }
        public int ChargersAmount { get; set; }
    }

}
