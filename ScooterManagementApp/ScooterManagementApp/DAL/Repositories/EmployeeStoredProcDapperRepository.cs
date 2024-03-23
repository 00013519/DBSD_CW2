using Dapper;
using Microsoft.Data.SqlClient;
using ScooterManagementApp.DAL.Models;
using System.Data;

namespace ScooterManagementApp.DAL.Repositories
{
    public class EmployeeStoredProcDapperRepository : IEmployeeRepository
    {
        private readonly string _connStr;

        public EmployeeStoredProcDapperRepository(string connStr)
        {
            _connStr = connStr;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using var conn = new SqlConnection(_connStr);
            return await conn.QueryAsync<Employee>(
                "udpGetAllEmployees",
                commandType: CommandType.StoredProcedure
                );
        }

        public IAsyncEnumerable<Employee> GetAllAsync2()
        {
            throw new NotImplementedException();
        }

        public Employee? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(Employee emp)
        {
            using var conn = new SqlConnection(_connStr);
            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(new
            {
                emp.FirstName,
                emp.LastName,
                emp.DateEmployed,
                emp.Position,
                emp.Salary,
                emp.IsActive,
                emp.ProfilePicture,
                emp.StationId
            });
            parameters.Add(
                "@Errors",
                direction: ParameterDirection.Output,
                dbType: DbType.String,
                size: 1000);
            parameters.Add(
                "RetVal",
                direction: ParameterDirection.ReturnValue,
                dbType: DbType.Int32);

            int id = conn.ExecuteScalar<int>(
                "udpInsertEmployee",
                commandType: CommandType.StoredProcedure,
                param: parameters
                );

            if (parameters.Get<int>("RetVal") != 0)
            {
                throw new Exception(parameters.Get<string>("Errors"));
            }

            return id;
        }

        public void Update(Employee emp)
        {
            throw new NotImplementedException();
        }
    }
}
