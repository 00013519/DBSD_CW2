using ScooterManagementApp.DAL.Models;
using X.PagedList;

namespace ScooterManagementApp.Models
{
    public class EmployeeFilterViewModel
    {
        public int? TotalCount { get; set; }
        public DateTime? Date { get; set; }
        public string? Position { get; set; }
        public int? StationId { get; set; }
        public string? SortField { get; set; }
        public bool SortDesc { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 4;
        public IPagedList<Employee> Employees { get; set; }
    }
}
