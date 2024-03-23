using ScooterManagementApp.DAL.Models;

namespace ScooterManagementApp.DAL.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        IAsyncEnumerable<Employee> GetAllAsync2();
        Employee? GetById(int id);
        int Insert(Employee emp);
        void Update(Employee emp);
        void Delete(int id);
    }
}
