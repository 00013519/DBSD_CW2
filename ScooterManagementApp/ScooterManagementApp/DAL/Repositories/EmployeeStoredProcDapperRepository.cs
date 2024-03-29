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

        public async Task<string> ExportToJson(DateTime? dateEmployed, string? position, int? stationId)
        {
            using var conn = new SqlConnection(_connStr);
            return await conn.ExecuteScalarAsync<string>(
                "udpEmployeeExportToJson",
                new
                {
                    DateEmployed = dateEmployed,
                    Position = position,
                    StationId = stationId
                }, commandType: CommandType.StoredProcedure) ?? "";
        }

        public async Task<string> ExportToXml(DateTime? dateEmployed, string? position, int? stationId)
        {
            using var conn = new SqlConnection(_connStr);
            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(new
            {
                DateEmployed = dateEmployed,
                Position = position,
                StationId = stationId
            });
            parameters.Add(
                "Results",
                direction: ParameterDirection.Output,
                dbType: DbType.String,
                size: int.MaxValue
                );

            await conn.ExecuteAsync(
                "udpEmployeeExportToXml",
                commandType: CommandType.StoredProcedure,
                param: parameters
                );

            return parameters.Get<string>("Results");
        }

        public async Task<(IEnumerable<Employee>, int)> Filter(
            DateTime? date, string? position, int? stationId,
            string? sortField = "EmployeeId", bool sortDesc = false,
            int page = 1, int pageSize = 2)
        {
            var orderBy = "EmployeeId";
            if ("DateEmployed".Equals(sortField))
                orderBy = "DateEmployed";
            else if ("StationId".Equals(sortField))
                orderBy = "StationId";
            else if ("Position".Equals(sortField))
                orderBy = "Position";

            var sql = "udpFilterEmployees";

            using var conn = new SqlConnection(_connStr);
            var list = await conn.QueryAsync<Employee>(
                sql,
                new
                {
                    DateEmployed = date,
                    Position = position,
                    StationId = stationId,
                    SortField = orderBy,
                    SortDesc = sortDesc,
                    Page = page,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            int totalCount = list?.FirstOrDefault()?.TotalCount ?? 0;

            return (list ?? Enumerable.Empty<Employee>(), totalCount);
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

        public async Task<IEnumerable<Employee>> ImportFromXml(string xml)
        {
            using var conn = new SqlConnection(_connStr);
            return await conn.QueryAsync<Employee>(
                 "udpEmployeeImportFromXml",
                 commandType: CommandType.StoredProcedure,
                 param: new { xml = xml }
                 );
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

            var result = await conn.QueryFirstOrDefaultAsync
                <(int EmployeeId, int ResultCode, string ErrorMessage)>
                ("udpInsertEmployee", parameters, commandType: CommandType.StoredProcedure);

            if (result.ResultCode != 0)
            {
                throw new Exception(result.ErrorMessage);
            }

            return result.EmployeeId;
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

            var result = await conn.QueryFirstOrDefaultAsync
                <(int ResultCode, string ErrorMessage)>
                ("udpUpdateEmployee", parameters, commandType: CommandType.StoredProcedure);

            if (result.ResultCode != 0)
            {
                throw new Exception(result.ErrorMessage);
            }
        }
    }
}
