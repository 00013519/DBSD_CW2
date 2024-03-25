namespace ScooterManagementApp.DAL.Models
{
    public class MaintenanceRecord
    {
        public int MaintenanceId { get; set; }
        public int EmployeeId { get; set; }
        public int ScooterId { get; set; }
        public required string IssueDescription { get; set; }
        public decimal RepairCost { get; set; }
        public required string RepairStatus { get; set; }
        public DateTime RepairDate { get; set; }
        public decimal? TotalRepairCost { get; set; }
    }

}
