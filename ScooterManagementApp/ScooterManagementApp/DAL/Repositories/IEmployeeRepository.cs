using ScooterManagementApp.DAL.Models;

namespace ScooterManagementApp.DAL.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetById(int id);
        Task<int> Insert(Employee emp);
        Task Update(Employee emp);
        Task Delete(int id);
    }
}
