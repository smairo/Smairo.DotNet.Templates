using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
namespace Smairo.Template.Model.Repositories
{
    public interface IDapperRepository
    {
        Task<string> GetSqlServerNameAsync();
    }

    public class DapperRepository : IDapperRepository
    {
        private readonly IConfiguration _configuration;
        public DapperRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetSqlServerNameAsync()
        {
            await using (var db = new SqlConnection(_configuration.GetConnectionString("DatabaseContext")))
            {
                return await db.QueryFirstAsync<string>("SELECT @@SERVERNAME");
            }
        }
    }
}