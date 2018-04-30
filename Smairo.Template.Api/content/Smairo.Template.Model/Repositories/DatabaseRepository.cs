using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace Smairo.Template.Model.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly string _connectionString;
        public DatabaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseContext");
        }
        public Task<IEnumerable<Entities.Sample>> GetSamplesAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString)) {
                return Task.FromResult(db.Query<Entities.Sample>("SELECT * FROM MySchema.Sample"));
            }
        }
    }

    public interface IDatabaseRepository
    {
        Task<IEnumerable<Entities.Sample>> GetSamplesAsync();
    }
}