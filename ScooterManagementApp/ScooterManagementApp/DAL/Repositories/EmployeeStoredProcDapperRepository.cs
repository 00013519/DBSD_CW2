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

        public async Task Delete(int eId)
        {
            using var conn = new SqlConnection(_connStr);
            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(new
            {
                EmployeeId = eId
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

            int id = await conn.ExecuteScalarAsync<int>(
                "udpDeleteEmployee",
                commandType: CommandType.StoredProcedure,
                param: parameters
                );

            if (parameters.Get<int>("RetVal") != 0)
            {
                throw new Exception(parameters.Get<string>("Errors"));
            }
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using var conn = new SqlConnection(_connStr);
            return (await conn.QueryAsync<Employee>(
                "udpGetAllEmployees",
                commandType: CommandType.StoredProcedure
                )).ToList();
        }

        public async Task<Employee?> GetById(int id)
        {
            var employees = await GetAllAsync();
            var emp = employees.FirstOrDefault(e=> e.EmployeeId == id);
            return emp;
        }

        public async Task<int> Insert(Employee emp)
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

            int id = await conn.ExecuteScalarAsync<int>(
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

        public async Task Update(Employee emp)
        {
            using var conn = new SqlConnection(_connStr);
            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(new
            {
                emp.EmployeeId,
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

            int id = await conn.ExecuteScalarAsync<int>(
                "udpUpdateEmployee",
                commandType: CommandType.StoredProcedure,
                param: parameters
                );

            if (parameters.Get<int>("RetVal") != 0)
            {
                throw new Exception(parameters.Get<string>("Errors"));
            }
        }
    }
}
