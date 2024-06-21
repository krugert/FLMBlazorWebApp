using FLMBlazorWebApp.Interface;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using FLMBlazorWebApp.Repositories;
using Dapper;

namespace FLMBlazorWebApp.Service
{
    public class ReportService : IReportService
    {
        public IConfiguration _Configuration { get; }
        public string _connectionString = "";

        public ReportService(IConfiguration configuration)
        {
            _Configuration = configuration;
            _connectionString = _Configuration.GetConnectionString("FLMDbConnection");
        }

        public async Task<IEnumerable<Report>> GetProductsByBranch()
        {
            IEnumerable<Report> result;

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                result = await connection.QueryAsync<Report>("dbo.GetProductsByBranch", commandType: System.Data.CommandType.StoredProcedure);
            }
            return result;
        }
    }
}
