namespace ScooterManagementApp.DAL.Models
{
    public class MaintenanceRecord
    {
        public int MaintenanceId { get; set; }
        public int EmployeeId { get; set; } // Foreign Key
        public Employee Employee { get; set; } // Navigation property
        public int ScooterId { get; set; } // Foreign Key
        public Scooter Scooter { get; set; } // Navigation property
        public string IssueDescription { get; set; }
        public decimal RepairCost { get; set; }
        public string RepairStatus { get; set; }
        public DateTime RepairDate { get; set; }
        public decimal TotalRepairCost { get; set; }
    }

}
