using ScooterManagementApp.DAL.Models;

namespace ScooterManagementApp.DAL.Repositories
{
    public interface IEmployeeRepository
    {
        //IEnumerable<Employee> ImportFromXml(string xml);
        //IEnumerable<Employee> ImportFromJson(string json);
        string ExportToXml(
            DateTime? dateEmployed,
            string? position,
            int? stationId);

        string ExportToJson(
            DateTime? dateEmployed,
            string? position,
            int? stationId);
        Task<(IEnumerable<Employee>,int)> Filter(
            DateTime? date = null, 
            string? position = null,
            int? stationId = null, 
            string? sortField = nameof(Employee.EmployeeId),
            bool sortDesc = false, 
            int page = 1, 
            int pageSize = 2);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetById(int id);
        Task<int> Insert(Employee emp);
        Task Update(Employee emp);
        Task Delete(int id);
    }
}
