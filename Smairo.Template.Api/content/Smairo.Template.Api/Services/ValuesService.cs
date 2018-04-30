using System.Collections.Generic;
using System.Threading.Tasks;
using Smairo.Template.Model.Repositories;
namespace Smairo.Template.Api.Services
{
    public class ValuesService : IValuesService
    {
        private readonly IDatabaseRepository _databaseRepository;
        public ValuesService(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }
        public async Task<IEnumerable<string>> GetValuesAsync()
        {
            //var samples = await _databaseRepository.GetSamplesAsync();
            return new string[] { "value1", "value2" };
        }

        public async Task<string> GetValueAsync(int id)
        {
            return id.ToString();
        }

        public async Task PostValuesAsync(string value)
        {
            return;
        }
    }

    public interface IValuesService
    {
        Task<IEnumerable<string>> GetValuesAsync();
        Task<string> GetValueAsync(int id);
        Task PostValuesAsync(string value);
    }
}