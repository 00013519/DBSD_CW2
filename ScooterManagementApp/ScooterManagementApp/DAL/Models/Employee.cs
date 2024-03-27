using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace ScooterManagementApp.DAL.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime DateEmployed { get; set; }
        public required string Position { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public int StationId { get; set; }
        public int? TotalCount { get; set; }
    }

}
