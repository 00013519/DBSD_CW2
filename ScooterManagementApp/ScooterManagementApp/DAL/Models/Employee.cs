namespace ScooterManagementApp.DAL.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateEmployed { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public byte[] ProfilePicture { get; set; }
        public int StationId { get; set; } // Foreign Key
        public Station Station { get; set; } // Navigation property
    }

}
